using System.Web.Mvc;
using System.Web.Routing;

namespace UpVotes
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("categories", "{categoriesname}/{id}",new { controller = "CompanyList", action = "CompanyList", id = UrlParameter.Optional }, new RouteValueDictionary { { "categoriesname", "mobile-application-developers|seo-companies|digital-marketing-companies|web-design-companies|software-development-companies|web-development-companies" } });
            routes.MapRoute("Profiles", "{Profile}/{id}", new { controller = "Company", action = "Company", id = UrlParameter.Optional }, new RouteValueDictionary { { "Profile", "Profile" } });
            routes.MapRoute("HomePage", "", new { controller = "Home", action = "HomePage", id = UrlParameter.Optional });
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "HomePage", id = UrlParameter.Optional }
            );
        }
    }
}
