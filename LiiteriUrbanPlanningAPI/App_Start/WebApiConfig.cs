using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

using LiiteriUrbanPlanningCore.Util;

namespace LiiteriUrbanPlanningAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            GlobalConfiguration.Configuration.BindParameter(
                typeof(DateRange), new Binders.DateRangeModelBinder());

            GlobalConfiguration.Configuration.BindParameter(
                typeof(int[]), new Binders.IntegerArrayModelBinder());

            GlobalConfiguration.Configuration.BindParameter(
                typeof(string[]), new Binders.StringArrayModelBinder());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
