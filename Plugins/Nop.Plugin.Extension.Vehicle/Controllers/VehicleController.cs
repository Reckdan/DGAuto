using Nop.Core;
using Nop.Core.Data;
using Nop.Plugin.Extension.Vehicle.Data;
using Nop.Plugin.Extension.Vehicle.Domain;
using Nop.Plugin.Extension.Vehicle.Services;
using Nop.Services.Catalog;
using Nop.Web.Framework.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Nop.Plugin.Extension.Vehicle.Controllers
{
   public  class VehicleController:BasePluginController
    {
        private readonly IVehicleRecordService _vehicleService;
        //private IRepository<VehicelDBObjectContext> _context;
        private readonly IProductService _productService;
      //  private IRepository<VehicleRecord> _vehicleRecord;
        private readonly IWorkContext _workContext;

        public VehicleController(IWorkContext workContext, IVehicleRecordService vehicleService,
            IProductService productService)
        {
            _vehicleService = vehicleService;
            _workContext = workContext;
            _productService = productService;
        }

        public ActionResult ListFitment(int? productID)
        {
            List < Fitment > fitments= _vehicleService.GetFitments(productID);
            return PartialView("_fitmentlist", fitments);
        }
        public ActionResult ListProducts(int vendorID)
        {
            var products= _productService.SearchProducts(vendorId: vendorID);
            return Json(products);
        }
        public ActionResult Manage()
        {
            return View();
        }

    }
}
