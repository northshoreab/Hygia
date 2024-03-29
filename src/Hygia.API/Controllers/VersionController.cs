﻿namespace Hygia.API.Controllers
{
    using System.Diagnostics;
    using System.Reflection;

    public class VersionController
    {
        public string get_Version()
        {            
            var version = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            return string.Format("{0}.{1}.{2}", version.FileMajorPart, version.FileMinorPart, version.FileBuildPart);
        }
    }
}