namespace Hygia.API.Controllers.Operations.LaunchPad
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Web.Http;
    using AttributeRouting;
    using AttributeRouting.Web.Http;

    [DefaultHttpRouteConvention]
    [RoutePrefix("api/operations/launchpad/download")]
    [Authorize]
    public class DownloadController : ApiController
    {
        static string configTemplate = @"<?xml version='1.0' encoding='utf-8' ?>
<configuration>
  <appSettings>
    <add key='watchr.apikey' value='{environment}'/>
    <add key='watchr.audit.input' value='audit'/>
    <add key='watchr.errors.input' value='error'/>
    <add key='watchr.apiurl' value='http://api.watchr.se'/>
  </appSettings>
</configuration>
";
        public HttpResponseMessage Get(Guid environmentId)
        {
            var baseDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_data");


            var tempDir = Path.Combine(baseDir, Guid.NewGuid().ToString());

            Directory.CreateDirectory(tempDir);

            var filePath = Path.Combine(baseDir, "WatchR.LaunchPad.zip");

            if(!File.Exists(filePath))
                throw new InvalidOperationException("No download template found at " + filePath);

            var template = Ionic.Zip.ZipFile.Read(filePath);

            //create the new config
            var configFile = Path.Combine(tempDir, "Hygia.LaunchPad.dll.config");
            var str = "\"";

            File.WriteAllText(configFile,configTemplate
                .Replace("'", str)
                .Replace("{environment}",environmentId.ToString()));

            //template.UpdateFile("Hygia.LaunchPad.dll.config");
            var memoryStream = new MemoryStream();

            template.UpdateFile(configFile,".\\");
            template.Save(memoryStream);

            memoryStream.Position = 0;
        
            var result = new HttpResponseMessage(HttpStatusCode.OK)
                             {
                                 Content = new StreamContent(memoryStream)
                             };
            
            //a text file is actually an octet-stream (pdf, etc)
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            
            //we used attachment to force download
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                                                            {
                                                                FileName = "WatchR.LaunchPad.zip"//todo put the env name in there
                                                            };
            
            return result;
        }

    }
}