using Nop.Plugin.Extension.Vehicle.Infrastructure;
using Nop.Web.Framework.Mvc.Routes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace Nop.Plugin.Extension.Vehicle
{
    public class RouteConfig : IRouteProvider
    {
       int IRouteProvider.Priority
        {
            get
            {
                return 0;
            }
        }

       public  void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute("Plugin.Extension.Vehicle.ManageVehicle",
                "Vehicle/Manage",
                new { Controller = "Vehicle", action = "Manage" },
                new[] { "Nop.Plugin.Extension.Vehicle.Controllers" });
            ViewEngines.Engines.Insert(0, new CustomViewEngine());

            routes.MapRoute("Plugin.Extension.Vehicle.ListFitment",
                "Vehicle/ListFitment",
                new { Controller = "Vehicle", action = "ListFitment" },
                new[] { "Nop.Plugin.Extension.Vehicle.Controllers" });
            ViewEngines.Engines.Insert(0, new CustomViewEngine());

            routes.MapRoute("Plugin.Extension.Vehicle.ListProduct",
               "Vehicle/ListProducts",
               new { Controller = "Vehicle", action = "ListProducts" },
               new[] { "Nop.Plugin.Extension.Vehicle.Controllers" });
            ViewEngines.Engines.Insert(0, new CustomViewEngine());
            //throw new NotImplementedException();
        }
    }
}
