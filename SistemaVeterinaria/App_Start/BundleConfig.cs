using System.Web;
using System.Web.Optimization;

namespace SistemaVeterinaria
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = true;
            ScriptBundle scriptBundle = new ScriptBundle("~/js");
            string[] scriptArray =
            {
                "~/Scripts/jquery-2.1.2.js",
                "~/Scripts/bootbox.js",
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js",
                "~/Scripts/select2.js",
                "~/Scripts/toastr.js",                
                "~/Scripts/DataTables/jquery.dataTables.js",
                "~/Scripts/DataTables/dataTables.bootstrap.js",
                "~/Scripts/jquery.steps.min.js"
        };
            scriptBundle.Include(scriptArray);
            scriptBundle.IncludeDirectory("~/Scripts/", "*.js");
            bundles.Add(scriptBundle);

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));



            bundles.Add(new StyleBundle("~/Styles/css").Include(
                                       "~/Content/bootstrap.css",
                                       "~/Content/site.css",
                                       "~/Content/css/select2.css",
                                       "~/Content/DataTables/css/dataTables.uikit.min.css",
                                       "~/Content/toastr.css",
                                       "~/Content/jquery.steps.css"
                     ));
        }
    }
}
