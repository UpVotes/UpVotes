using System.Web;
using System.Web.Optimization;

namespace UpVotes
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Clear();
            bundles.ResetAll();
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/media.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/script/files").Include(
                      "~/Scripts/umd/popper.js",
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/media.js",
                      "~/Scripts/UserScripts/Common.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/bootstrap.css",
                    "~/Content/header-footer.css",
                    "~/Content/common.css"));

            bundles.Add(new StyleBundle("~/Content/detail").Include("~/Content/details.css"));
            bundles.Add(new ScriptBundle("~/bundles/AllListing").Include("~/Scripts/UserScripts/AllListPages.js"));

            bundles.Add(new StyleBundle("~/Content/Anews").Include("~/Content/text-editor.css"));
            bundles.Add(new ScriptBundle("~/bundles/Anews").Include("~/Scripts/text-editor.js", "~/Scripts/UserScripts/News.js"));
            bundles.Add(new ScriptBundle("~/bundles/Unews").Include("~/Scripts/user-text-editor.js", "~/Scripts/UserScripts/News.js"));

            bundles.Add(new ScriptBundle("~/bundles/claimlist").Include("~/Scripts/UserScripts/UserCompanyList.js"));

            bundles.Add(new StyleBundle("~/Content/UserCompany").Include("~/Content/text-editor.css"));
            bundles.Add(new ScriptBundle("~/bundles/UserCompany").Include("~/Scripts/user-text-editor.js", "~/Scripts/UserScripts/UserCompanyList.js"));
            bundles.Add(new ScriptBundle("~/bundles/AUserCompany").Include("~/Scripts/text-editor.js", "~/Scripts/UserScripts/UserCompanyList.js"));

            bundles.Add(new ScriptBundle("~/bundles/UserCompanyList").Include("~/Scripts/UserScripts/UserCompanyList.js"));

            bundles.Add(new StyleBundle("~/Content/UserDashboard").Include("~/Content/dashboard.css"));
            bundles.Add(new ScriptBundle("~/bundles/UserDashboard").Include("~/Scripts/UserScripts/Dashboard.js"));
            bundles.Add(new ScriptBundle("~/bundles/ADashboard").Include("~/Scripts/UserScripts/Dashboard.js", "~/Scripts/UserScripts/AdminDashboard.js"));

            bundles.Add(new ScriptBundle("~/bundles/review").Include("~/Scripts/UserScripts/SubmitReview.js"));

            bundles.Add(new ScriptBundle("~/bundles/CompanyProfiles").Include(
                      "~/Scripts/highcharts.js",
                      "~/Scripts/UserScripts/Company.js",
                      "~/Scripts/UserScripts/CompanyVote.min.js"));
            
            bundles.Add(new StyleBundle("~/Content/CompList").Include("~/Content/animation.css", "~/Content/listing.css"));
            bundles.Add(new ScriptBundle("~/bundles/CompList").Include(
                      "~/Scripts/highcharts.js",
                      "~/Scripts/UserScripts/CompanyList.js"));

            bundles.Add(new StyleBundle("~/Content/Homecss").Include("~/Content/owl.carousel.css", "~/Content/home.css"));
            bundles.Add(new ScriptBundle("~/bundles/homejs").Include("~/Scripts/owl.carousel.min.js"));

            bundles.Add(new StyleBundle("~/Content/GetListedcss").Include("~/Content/GetListed.css"));

            bundles.Add(new ScriptBundle("~/bundles/login").Include("~/Scripts/UserScripts/Login.js"));

            bundles.Add(new ScriptBundle("~/bundles/softlist").Include("~/Scripts/UserScripts/SoftwareList.js"));

            bundles.Add(new ScriptBundle("~/bundles/software").Include("~/Scripts/UserScripts/Software.js"));

            bundles.Add(new ScriptBundle("~/bundles/allcategories").Include("~/Scripts/UserScripts/AllCategory.js"));

            bundles.Add(new ScriptBundle("~/bundles/CompanyPortfolio").Include("~/Scripts/UserScripts/UserPortfolioList.js"));

            bundles.Add(new ScriptBundle("~/bundles/ContactUs").Include("~/Scripts/UserScripts/ContactUs.js"));

            bundles.Add(new ScriptBundle("~/bundles/TeamMembersList").Include("~/Scripts/UserScripts/TeamMembers.js"));

            bundles.Add(new StyleBundle("~/Content/TeamMembers").Include("~/Content/bootstrap-datepicker.css"));
            bundles.Add(new ScriptBundle("~/bundles/TeamMembers").Include("~/Scripts/bootstrap-datepicker.js", "~/Scripts/UserScripts/TeamMembers.js"));

            bundles.Add(new ScriptBundle("~/bundles/UserReviewApproval").Include("~/Scripts/UserScripts/UserReviewForApproval.js"));

            bundles.Add(new StyleBundle("~/Content/Sponsorship").Include("~/Content/bootstrap-datepicker.css"));
            bundles.Add(new ScriptBundle("~/bundles/Sponsorship").Include("~/Scripts/bootstrap-datepicker.js", "~/Scripts/UserScripts/SponsorShip.js"));

            bundles.Add(new ScriptBundle("~/bundles/Quotation").Include(
                      "~/Scripts/UserScripts/Quotation.js"));
            
            //BundleTable.Bundles.UseCdn = true;

                       
            

            //bundles.Add(new StyleBundle("~/Content/Quotationcss").Include("~/Content/Quotation.css"));
            bundles.Add(new StyleBundle("~/Content/Companycss").Include("~/Content/Company.css"));
            bundles.Add(new ScriptBundle("~/bundles/CompanyProfiles").Include(
                      "~/Scripts/highcharts.js",
                      "~/Scripts/UserScripts/Company.js",
                      "~/Scripts/UserScripts/CompanyVote.min.js"));

            bundles.Add(new StyleBundle("~/Content/UserListCss").Include("~/Content/text-editor.css",
                "~/Content/multi-select.css"));

            bundles.Add(new ScriptBundle("~/bundles/AUserSoftwareListPluginJs").Include("~/Scripts/multi-select.js","~/Scripts/text-editor.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserSoftwareListPluginJs").Include("~/Scripts/multi-select.js", "~/Scripts/user-text-editor.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserSoftwareList").Include("~/Scripts/UserScripts/UserSoftwareList.js"));

            BundleTable.EnableOptimizations = true;
        }
    }
}
