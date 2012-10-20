using System.Web.Optimization;

namespace Hygia.Spa.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Force optimization to be on or off, regardless of web.config setting
            //BundleTable.EnableOptimizations = false;
            bundles.UseCdn = false;

            // .debug.js, -vsdoc.js and .intellisense.js files 
            // are in BundleTable.Bundles.IgnoreList by default.
            // Clear out the list and add back the ones we want to ignore.
            // Don't add back .debug.js.
            bundles.IgnoreList.Clear();
            bundles.IgnoreList.Ignore("*-vsdoc.js");
            bundles.IgnoreList.Ignore("*intellisense.js");

            // Modernizr goes separate since it loads first
            bundles.Add(new ScriptBundle("~/bundles/modernizr")
                .Include("~/Scripts/lib/modernizr-{version}.js"));

            // jQuery
            bundles.Add(new ScriptBundle("~/bundles/jquery",
                "//ajax.googleapis.com/ajax/libs/jquery/1.8.1/jquery.min.js")
                .Include("~/Scripts/lib/jquery-{version}.js"));

            // jQuery UI
            bundles.Add(new ScriptBundle("~/bundles/jqueryui",
                "//ajax.aspnetcdn.com/ajax/jquery.ui/1.8.23/jquery-ui.min.js")
                .Include("~/Scripts/lib/jquery-ui-{version}.js"));

            // Wijmo
            bundles.Add(new ScriptBundle("~/bundles/wijmocomplete",
                "http://cdn.wijmo.com/jquery.wijmo-complete.all.2.2.1.min.js")
                .Include("~/Scripts/lib/jquery.wijmo-complete.all.2.2.1.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/wijmoopen",
                "http://cdn.wijmo.com/jquery.wijmo-open.all.2.2.1.min.js")
                .Include("~/Scripts/lib/jquery.wijmo-open.all.2.2.1.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/wijmoknockout",
                "http://cdn.wijmo.com/external/knockout.wijmo.js")
                .Include("~/Scripts/lib/knockout.wijmo.js"));

            // 3rd Party JavaScript files
            bundles.Add(new ScriptBundle("~/bundles/jsextlibs")
                //.IncludeDirectory("~/Scripts/lib", "*.js", searchSubdirectories: false));
                .Include(
                    "~/Scripts/lib/json2.js", // IE7 needs this

                    // jQuery plugins
                    //"~/Scripts/lib/activity-indicator.js",
                    "~/Scripts/lib/jquery.mockjson.js",
                    "~/Scripts/lib/TrafficCop.js",
                    "~/Scripts/lib/infuser.js", // depends on TrafficCop,

                    // Knockout and its plugins
                    "~/Scripts/lib/knockout-{version}.js",
                    "~/Scripts/lib/knockout.activity.js",
                    "~/Scripts/lib/knockout.asyncCommand.js",
                    "~/Scripts/lib/knockout.dirtyFlag.js",
                    "~/Scripts/lib/knockout.validation.js",
                    "~/Scripts/lib/koExternalTemplateEngine.js",
                    "~/Scripts/lib/bootstrap.js",
                    // Other 3rd party libraries
                    "~/Scripts/lib/underscore.js",
                    "~/Scripts/lib/moment.js",
                    "~/Scripts/lib/sammy.*",
                    "~/Scripts/lib/amplify.*"
                    //"~/Scripts/lib/toastr.js"
                    ));
            bundles.Add(new ScriptBundle("~/bundles/jsmocks")
                .IncludeDirectory("~/Scripts/app/mock", "*.js", searchSubdirectories: false));

            // All application JS files (except mocks)
            bundles.Add(new ScriptBundle("~/bundles/jsapplibs")
                .IncludeDirectory("~/Scripts/app/", "*.js", searchSubdirectories: false)
                .IncludeDirectory("~/Scripts/app/model", "*.js", searchSubdirectories: false)
                .IncludeDirectory("~/Scripts/app/infrastructure", "*.js", searchSubdirectories: false)
                .IncludeDirectory("~/Scripts/app/viewModel", "*.js", searchSubdirectories: false));

            // 3rd Party CSS files
            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/main.css",
                "~/Content/normalize.css",
                "~/Content/bootstrap.css",
                "~/Content/bootstrap-resonsive.css"
                //"~/Content/toastr.css",
                //"~/Content/toastr-responsive.css"
                ));

            bundles.Add(new StyleBundle("~/Content/css/wijmocomplete",
                "http://cdn.wijmo.com/jquery.wijmo-complete.all.2.2.1.min.css")
                .Include("~/Content/jquery.wijmo-complete.all.2.2.1.min.css"));

            bundles.Add(new StyleBundle("~/Content/css/wijmoaristo",
                "http://cdn.wijmo.com/themes/aristo/jquery-wijmo.css")
                .Include("~/Content/jquery-wijmo.css"));

            // Custom LESS files
            //bundles.Add(new Bundle("~/Content/Less", new LessTransform(), new CssMinify())
            //    .Include("~/Content/styles.less"));
        }
    }
}