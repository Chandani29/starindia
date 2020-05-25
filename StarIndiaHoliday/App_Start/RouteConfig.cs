using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace StarIndiaHoliday
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
              name: "Default1",
              url: "Admin/{action}/{id}",
              defaults: new { controller = "Admin", action = "Login", id = UrlParameter.Optional }
          );

            routes.MapRoute(
                name: "Default",
                url: "{action}/{tourtype}/{tourname}",
                defaults: new { controller = "Starindiaholiday", action = "Index", tourtype = UrlParameter.Optional, tourname = UrlParameter.Optional }
            );


        }
    }
}
