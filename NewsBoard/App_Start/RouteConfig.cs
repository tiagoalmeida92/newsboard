using System.Web.Mvc;
using System.Web.Routing;

namespace NewsBoard.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("News", "News/{category}",
                new {Controller = "News", action = "Index", category = UrlParameter.Optional}
                );

            routes.MapRoute("Admin", "Admin/News/{category}",
                new {Controller = "Admin", action = "News", category = UrlParameter.Optional}
                );

            routes.MapRoute("Default", "{controller}/{action}/{id}",
                new {controller = "News", action = "Index", id = UrlParameter.Optional}
                );
        }
    }
}