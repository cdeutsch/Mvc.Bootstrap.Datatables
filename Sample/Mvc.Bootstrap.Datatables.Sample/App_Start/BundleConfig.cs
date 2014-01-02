using System.Web;
using System.Web.Optimization;

namespace Mvc.Bootstrap.Datatables.Sample
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles (BundleCollection bundles)
        {
            bundles.Add (new ScriptBundle ("~/bundles/default").Include (
                        "~/Scripts/jquery-{version}.js",
                        "~/scripts/underscore*",
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*",
                        "~/scripts/jquery.dataTables.*",
                        "~/scripts/jquery.ext.dataTables.*",
                        "~/scripts/bootstrap.*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add (new ScriptBundle ("~/bundles/modernizr").Include (
                        "~/Scripts/modernizr-*"));

            bundles.Add (new StyleBundle ("~/Content/css").Include (
                "~/Content/bootstrap.css",
                "~/Content/bootstrap-theme.css",
                "~/Content/bootstrap.datatables.css",
                "~/Content/site.css"));

        }
    }
}