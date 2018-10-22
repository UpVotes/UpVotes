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
            routes.MapRoute("subcategories", "{categoriesname}/{subcategory}/{id}", new { controller = "CompanyList", action = "CompanySubCategoryList", id = UrlParameter.Optional }, new RouteValueDictionary { { "categoriesname", "mobile-application-developers|web-development-companies|wearable-application-developers|software-development-companies|ecommerce-developers|seo-companies|social-media-marketing-companies|ppc-companies" }, { "subcategory", "android|windows|iphone|ipad|hybrid|wordpress|drupal|joomla|sitecore|php|javascript|angular-js|ajax|python|cakephp|codeigniter|laravel|zend|symfony|yii|django|apple-watch|android-watches|google-glass|dot-net|java|ruby-on-rails|magento|shopify|woocommerce|on-page-optimization|local-search|link-building|content-development|mobile-optimization|facebook-advertising|twitter-advertising|linkedIn-advertising|instagram-advertising|influencers-advertising|google-advertising|bing-advertising|yahoo-advertising|youtube-advertising" } });

            routes.MapRoute("categories", "{categoriesname}/{id}",new { controller = "CompanyList", action = "CompanyList", id = UrlParameter.Optional }, new RouteValueDictionary { { "categoriesname", "mobile-application-developers|seo-companies|digital-marketing-companies|web-design-companies|software-development-companies|web-development-companies|ui-ux-agencies|wearable-application-developers|ecommerce-developers|social-media-marketing-companies|ppc-companies|content-marketing-companies|iot-application-developers|mobile-app-marketing-agencies|email-marketing-agencies|sem-agencies|branding-agencies|pr-agencies|digital-strategy-agencies|video-production-agencies|chatbot-development|ar-vr-application-development|graphic-designers" } });
            routes.MapRoute("Profiles", "{Profile}/{id}", new { controller = "Company", action = "Company", id = UrlParameter.Optional }, new RouteValueDictionary { { "Profile", "Profile" } });
            routes.MapRoute("blogs", "{Blog}/{id}", new { controller = "blogs", action = "blog", id = UrlParameter.Optional }, new RouteValueDictionary { { "Blog", "resources" } });
            routes.MapRoute("userCompany", "company/my-profile/{id}", new { controller = "UserCompanyList", action = "UserCompanyList", id = UrlParameter.Optional });
            routes.MapRoute("claimCompany", "company/claimcompanyverification/{id}", new { controller = "UserCompanyList", action = "CompanyClaimVerificationByUser", id = UrlParameter.Optional });
            routes.MapRoute("privatePolicy", "privacy-policy/{id}", new { controller = "Home", action = "PrivatePolicy", id = UrlParameter.Optional });
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
