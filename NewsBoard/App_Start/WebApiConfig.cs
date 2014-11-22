using System.Web.Http;
using System.Web.Http.OData.Builder;
using NewsBoard.Model;

namespace NewsBoard.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "NewsCategories",
            //    routeTemplate: "api/{controller}/{category}",
            //    defaults: new { }//id = RouteParameter.Optional }
            //);

            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new {id = RouteParameter.Optional}
                );

            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<NewsItem>("NewsItems");
            builder.EntitySet<NewsSource>("NewsSources");
            config.Routes.MapODataRoute("odata", "odata", builder.GetEdmModel());
        }
    }
}