using Nop.Web.Framework.Mvc.Routes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Nop.Admin.Infrastructure
{
    public class RouteProvider : IRouteProvider
    {
        int IRouteProvider.Priority
        {
            get
            {
                return 1;
            }
        }

        void IRouteProvider.RegisterRoutes(RouteCollection routes)
        {
            var route = routes.MapRoute("Nop.Admin.Fitment.Index",
                                 "Admin/Fitment/Index",
                                  new { controller = "Fitment", action = "Index", area = "" },
                                  new[] { "Nop.Admin.Controllers" });
            routes.Remove(route);
            routes.Insert(0, route);
        }
    }
}