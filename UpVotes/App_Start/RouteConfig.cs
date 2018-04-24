﻿using System.Web.Mvc;
using System.Web.Routing;

namespace UpVotes
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("categories", "{categoriesname}/{id}",new { controller = "CompanyList", action = "CompanyList", id = UrlParameter.Optional }, new RouteValueDictionary { { "categoriesname", "mobile-application-developers|seo-companies|digital-marketing-companies|web-design-companies|software-development-companies|web-development-companies|ui-ux-agencies|wearable-application-developers|ecommerce-developers|social-media-marketing-companies|ppc-companies|content-marketing-companies|iot-application-developers" } });
            //routes.MapRoute("categories", "{categoriesname}",new { controller = "CompanyList", action = "CompanyList" }, new RouteValueDictionary { { "categoriesname", "mobile-application-developers|seo-companies|digital-marketing-companies|web-design-companies|software-development-companies|web-development-companies|ui-ux-agencies|wearable-application-developers|ecommerce-developers|social-media-marketing-companies|ppc-companies|content-marketing-companies|iot-application-developers" } });
            //routes.MapRoute("mobilesubcategories", "mobile-application-developers/{id}", new { controller = "Resources", action = "ResourceSub", id = UrlParameter.Optional }, new { id = "(Microsoft)|(Apple)" });
            //routes.MapRoute("mobilelocationcategories", "mobile-application-developers/{id}", new { controller = "Resources", action = "Resource", id = UrlParameter.Optional });
            routes.MapRoute("Profiles", "{Profile}/{id}", new { controller = "Company", action = "Company", id = UrlParameter.Optional }, new RouteValueDictionary { { "Profile", "Profile" } });
            routes.MapRoute("resources", "{resources}", new { controller = "Resources", action = "Resource" }, new RouteValueDictionary { { "resources", "resources" } });
            routes.MapRoute("resourceschild", "resources/{id}", new { controller = "Resources", action = "ResourceChild", id = UrlParameter.Optional }, new { id= "(how-much-cost-to-make-an-mobile-app)" });
            routes.MapRoute("HomePage", "", new { controller = "Home", action = "HomePage", id = UrlParameter.Optional });
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "HomePage", id = UrlParameter.Optional }
            );
        }
    }
}
