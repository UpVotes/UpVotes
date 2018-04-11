using System.Web;
using System.Web.Optimization;

namespace UpVotes
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/media.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));
            bundles.Add(new ScriptBundle("~/bundles/Quotation").Include(
                      "~/Scripts/UserScripts/Quotation.js"));
            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/bootstrap-theme.min.css",
            //          "~/Content/wvr - styles.css",
            //          "~/Content/site.css"));
            BundleTable.Bundles.UseCdn = true;
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/wvr-styles.css"));            
            bundles.Add(new StyleBundle("~/Content/Homecss").Include("~/Content/home-design.css"));
            bundles.Add(new StyleBundle("~/Content/Quotationcss").Include("~/Content/Quotation.css"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/media.js"));

            BundleTable.EnableOptimizations = true;
        }
    }
}
