using Nop.Core.Data;
using Nop.Core.Plugins;
using Nop.Plugin.Extension.Vehicle.Data;
using Nop.Plugin.Extension.Vehicle.Domain;
using Nop.Web.Framework;
using Nop.Web.Framework.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;

namespace Nop.Plugin.Extension.Vehicle
{
    public class VehiclePlugin : BasePlugin, IAdminMenuPlugin
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

        void IAdminMenuPlugin.ManageSiteMap(SiteMapNode rootNode)
        {
            var menuItem = new SiteMapNode()
            {
                SystemName = "Vehicles",
                Title = "Vehicles Setup",
                ControllerName = "Vehicle",
                ActionName = "Manage",
                Visible = true,
                RouteValues = new RouteValueDictionary() { { "area", null } },


            };
            var pluginNode = rootNode.ChildNodes.FirstOrDefault(x => x.SystemName == "Third party plugins");
            if (pluginNode != null)
            {
                pluginNode.ChildNodes.Add(menuItem);
            }
            else
                rootNode.ChildNodes.Add(menuItem);
        }
    }
}
