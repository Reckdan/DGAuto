using Nop.Core.Data;
using Nop.Core.Plugins;
using Nop.Plugin.Extension.Vehicle.Data;
using Nop.Plugin.Extension.Vehicle.Domain;
using Nop.Services.Cms;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;

namespace Nop.Plugin.Extension.Vehicle
{
    [AdminAuthorize]
    public class VehiclePlugin : BasePlugin, IWidgetPlugin
    {
        private readonly VehicelDBObjectContext _context;
        //private IRepository<Make> _makeRepo;
        //private IRepository<VehicleModel> _modelRepo;
        //private IRepository<Engine> _engineRepo;
        //private IRepository<VehicleRecord> _vehicleRepo;
        //private IRepository<VehicleType> _vehicleTypeRepo;
        //private IRepository<VehicleTypeGroup> _vehicleTypeGroupRepo;
        //private IRepository<BaseVehicle> _baseVehicleRepo;

        public VehiclePlugin(VehicelDBObjectContext context)
            
            //, IRepository<Make> make, IRepository<VehicleModel> model, IRepository<Engine> engine, IRepository<VehicleType> vehicletype,IRepository<VehicleTypeGroup> vehicleTypeGroup,IRepository<BaseVehicle> basevehicle,IRepository<VehicleRecord> vehiclerecord)
        {
            _context = context;
            //_makeRepo = make;
            //_modelRepo = model;
            //_vehicleRepo = vehiclerecord;
            //_vehicleTypeRepo = vehicletype;
            //_vehicleTypeGroupRepo = vehicleTypeGroup;
            //_engineRepo = engine;
            //_baseVehicleRepo = basevehicle;
        }
    

        
        public override void Install()
        {
            _context.Install();
            base.Install();
        }

        public override void Uninstall()
        {
            _context.Uninstall();
            base.Uninstall();
        }

        void IWidgetPlugin.GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "Configure";
            controllerName = "Fitment";
            routeValues = new RouteValueDictionary()
            {
                { "Namespaces", "Nop.Plugin.Extension.Vehicle.Controllers" },
                { "area", null }
            };
        }

        void IWidgetPlugin.GetDisplayWidgetRoute(string widgetZone, out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            if (widgetZone == "left_side_of_page")
            {
                actionName = "SearchFitment";
                controllerName = "Fitment";
                routeValues = new RouteValueDictionary
            {
                {"Namespaces", "Nop.Plugin.Extension.Vehicle.Controllers"},
                {"area", null},
                {"widgetZone", widgetZone}
            };
            }else if(widgetZone== "top_of_page")
            {
                actionName = "CheckFit";
                controllerName = "Fitment";
                routeValues = new RouteValueDictionary
            {
                {"Namespaces", "Nop.Plugin.Extension.Vehicle.Controllers"},
                {"area", null},
                {"widgetZone", widgetZone}
            };
            }
            else
            {
                actionName = "SearchFitment";
                controllerName = "Fitment";
                routeValues = new RouteValueDictionary
            {
                {"Namespaces", "Nop.Plugin.Extension.Vehicle.Controllers"},
                {"area", null},
                {"widgetZone", widgetZone}
            };
            }
        }

        IList<string> IWidgetPlugin.GetWidgetZones()
        {
            return new List<string>() { "left_side_of_page","top_of_page" };
        }

        //void IAdminMenuPlugin.ManageSiteMap(SiteMapNode rootNode)
        //{
        //    var menuItem = new SiteMapNode()
        //    {
        //        SystemName = "Vehicles",
        //        Title = "Vehicles Setup",
        //        ControllerName = "Vehicle",
        //        ActionName = "Manage",
        //        Visible = true,
        //        RouteValues = new RouteValueDictionary() { { "area", null } },


        //    };
        //    var pluginNode = rootNode.ChildNodes.FirstOrDefault(x => x.SystemName == "Third party plugins");
        //    if (pluginNode != null)
        //    {
        //        pluginNode.ChildNodes.Add(menuItem);
        //    }
        //    else
        //        rootNode.ChildNodes.Add(menuItem);
        //}
    }
}
