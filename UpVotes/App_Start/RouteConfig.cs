using System.Web.Mvc;
using System.Web.Routing;

namespace UpVotes
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.LowercaseUrls = true;
            routes.MapRoute("subcategories", "{categoriesname}/{subcategory}/{id}", new { controller = "CompanyList", action = "CompanySubCategoryList", id = UrlParameter.Optional }, new RouteValueDictionary { { "categoriesname", "mobile-application-developers|web-development-companies|wearable-application-developers|software-development-companies|ecommerce-developers|seo-companies|social-media-marketing-companies|ppc-companies|human-resources-companies|video-production-agencies|pr-agencies|cloud-consulting-companies" }, { "subcategory", "android|windows|iphone|ipad|hybrid|wordpress|drupal|joomla|sitecore|php|javascript|angular-js|ajax|python|cakephp|codeigniter|laravel|zend|symfony|yii|django|apple-watch|android-watches|google-glass|dot-net|java|ruby-on-rails|magento|shopify|woocommerce|on-site-optimization|local-search|link-building|content-development|mobile-optimization|facebook-advertising|twitter-advertising|linkedIn-advertising|instagram-advertising|influencers-advertising|google-advertising|bing-advertising|yahoo-advertising|youtube-advertising|staffing|recruiting|3d-animation|corporate|whiteboard|reputation-management|corporate-reputation|event-management|google-cloud-partners|aws-partners|azure-partners" } });

            routes.MapRoute("categories", "{categoriesname}/{id}",new { controller = "CompanyList", action = "CompanyList", id = UrlParameter.Optional }, new RouteValueDictionary { { "categoriesname", "mobile-application-developers|seo-companies|digital-marketing-companies|web-design-companies|software-development-companies|web-development-companies|ui-ux-agencies|wearable-application-developers|ecommerce-developers|social-media-marketing-companies|ppc-companies|content-marketing-companies|iot-application-developers|mobile-app-marketing-agencies|email-marketing-agencies|sem-agencies|branding-agencies|pr-agencies|digital-strategy-agencies|video-production-agencies|chatbot-development|ar-vr-application-development|graphic-designers|human-resources-companies|blockchain-developers|software-testing-companies|logo-design-agencies|print-design-agencies|product-design-agencies|packaging-design-agencies|affiliate-marketing-agencies|artificial-intelligence-companies|cloud-consulting-companies" } });
            routes.MapRoute("softwarecategories", "{softwarecategoriesname}", new { controller = "SoftwareList", action = "SoftwareList" }, new RouteValueDictionary { { "softwarecategoriesname", "mobile-app-softwares|marketing-softwares|seo-softwares|social-media-softwares|content-marketing-softwares|app-design-softwares|cms-softwares|ecommerce-softwares|email-marketing-softwares|game-development-softwares|graphic-design-softwares|iot-softwares|artificial-intelligence-softwares|data-visualization-softwares|inventory-management-softwares|accounting-softwares|workflow-management-softwares|crm-softwares|business-intelligence-softwares|erp-softwares|human-resource-softwares|machine-learning-softwares|product-management-softwares" } });
            routes.MapRoute("ProfilesNews", "{Profile}/{id}/news", new { controller = "OverViewAndNews", action = "CompanyNewsByName", id = UrlParameter.Optional }, new RouteValueDictionary { { "Profile", "Profile" } });
            routes.MapRoute("ProfilesReviews", "{Profile}/{id}/reviews", new { controller = "Company", action = "CompanyAllReviewsByName", id = UrlParameter.Optional }, new RouteValueDictionary { { "Profile", "Profile" } });
            routes.MapRoute("ProfilesPortFolio", "{Profile}/{id}/portfolio", new { controller = "Company", action = "CompanyAllPortfolioByName", id = UrlParameter.Optional }, new RouteValueDictionary { { "Profile", "Profile" } });
            routes.MapRoute("ProfilesTeamMembers", "{Profile}/{id}/team-members", new { controller = "Company", action = "CompanyAllTeamMembersByName", id = UrlParameter.Optional }, new RouteValueDictionary { { "Profile", "Profile" } });
            routes.MapRoute("Profiles", "{Profile}/{id}", new { controller = "Company", action = "Company", id = UrlParameter.Optional }, new RouteValueDictionary { { "Profile", "Profile" } });
            routes.MapRoute("SoftwaresNews", "{Software}/{id}/news", new { controller = "OverViewAndNews", action = "SoftwareNewsByName", id = UrlParameter.Optional }, new RouteValueDictionary { { "Software", "Software" } });
            routes.MapRoute("SoftwaresReviews", "{Software}/{id}/reviews", new { controller = "Softwares", action = "SoftwareAllReviewsByName", id = UrlParameter.Optional }, new RouteValueDictionary { { "Software", "Software" } });
            routes.MapRoute("SoftwareTeamMembers", "{Software}/{id}/team-members", new { controller = "Softwares", action = "CompanyAllTeamMembersByName", id = UrlParameter.Optional }, new RouteValueDictionary { { "Software", "Software" } });
            routes.MapRoute("Softwares", "{Software}/{id}", new { controller = "Softwares", action = "Softwares", id = UrlParameter.Optional }, new RouteValueDictionary { { "Software", "Software" } });            
            routes.MapRoute("blogs", "{Blog}/{id}", new { controller = "blogs", action = "blog", id = UrlParameter.Optional }, new RouteValueDictionary { { "Blog", "resources" } });
            routes.MapRoute("Article", "blog/{id}", new { controller = "blogs", action = "article", id = UrlParameter.Optional });
            routes.MapRoute("navigation", "all-categories/{id}", new { controller = "blogs", action = "AllCategories", id = UrlParameter.Optional });
            routes.MapRoute("userCompany", "company/my-dashboard/{id}", new { controller = "UserCompanyList", action = "UserDashboard", id = UrlParameter.Optional });
            //routes.MapRoute("userDashboard", "company/my-profile/{id}", new { controller = "UserCompanyList", action = "UserCompanyList", id = UrlParameter.Optional });
            routes.MapRoute("claimCompany", "company/claimcompanyverification/{id}", new { controller = "UserCompanyList", action = "CompanyClaimVerificationByUser", id = UrlParameter.Optional });
            routes.MapRoute("claimSoftware", "Softwares/claimsoftwareverification/{id}", new { controller = "UserCompanyList", action = "SoftwareClaimVerificationByUser", id = UrlParameter.Optional });
            routes.MapRoute("SubmitReview", "submit/submit-review/{id}", new { controller = "SubmitReview", action = "SubmitReview", id = UrlParameter.Optional });
            routes.MapRoute("privatePolicy", "privacy-policy/{id}", new { controller = "Home", action = "PrivatePolicy", id = UrlParameter.Optional });
            routes.MapRoute("writeForUs", "write-for-us/{id}", new { controller = "Home", action = "WriteForUs", id = UrlParameter.Optional });
            routes.MapRoute("sponsorship", "sponsorship/{id}", new { controller = "Home", action = "Sponsorship", id = UrlParameter.Optional });
            routes.MapRoute("contactUs", "contact-us/{id}", new { controller = "ContactUs", action = "ContactUsForm", id = UrlParameter.Optional });
            routes.MapRoute("newsDetail", "news/{id}", new { controller = "OverViewAndNews", action = "DetailNews", id = UrlParameter.Optional });
            routes.MapRoute("GetListed", "get-listed", new { controller = "Home", action = "GetListedPage", id = UrlParameter.Optional });
            //routes.MapRoute("categories", "{categoriesname}",new { controller = "CompanyList", action = "CompanyList" }, new RouteValueDictionary { { "categoriesname", "mobile-application-developers|seo-companies|digital-marketing-companies|web-design-companies|software-development-companies|web-development-companies|ui-ux-agencies|wearable-application-developers|ecommerce-developers|social-media-marketing-companies|ppc-companies|content-marketing-companies|iot-application-developers" } });
            //routes.MapRoute("mobilesubcategories", "mobile-application-developers/{id}", new { controller = "Resources", action = "ResourceSub", id = UrlParameter.Optional }, new { id = "(Microsoft)|(Apple)" });
            //routes.MapRoute("mobilelocationcategories", "mobile-application-developers/{id}", new { controller = "Resources", action = "Resource", id = UrlParameter.Optional });
            //routes.MapRoute("resources", "{resources}", new { controller = "Resources", action = "Resource" }, new RouteValueDictionary { { "resources", "resources" } });
            //routes.MapRoute("resourceschild", "resources/{id}", new { controller = "Resources", action = "ResourceChild", id = UrlParameter.Optional }, new { id= "(how-much-cost-to-make-an-mobile-app)" });
            routes.MapRoute("HomePage", "", new { controller = "Home", action = "HomePage", id = UrlParameter.Optional });            
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "HomePage", id = UrlParameter.Optional }
            );
        }
    }
}
