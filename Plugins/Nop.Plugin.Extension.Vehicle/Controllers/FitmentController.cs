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
using System.Net;
using System.Web.Mvc;
using Nop.Web.Framework.Kendoui;

namespace Nop.Plugin.Extension.Vehicle.Controllers
{
    public class FitmentController : BasePluginController
    {

        private readonly IVehicleRecordService _vehicleService;
        //private IRepository<VehicelDBObjectContext> _context;
        private readonly IProductService _productService;
        //  private IRepository<VehicleRecord> _vehicleRecord;
        private readonly IWorkContext _workContext;

        public FitmentController(IWorkContext workContext, IVehicleRecordService vehicleService,
            IProductService productService)
        {
            _vehicleService = vehicleService;
            _workContext = workContext;
            _productService = productService;
        }

        public ActionResult FitmentList(DataSourceRequest command, Fitment model)
        {
            //var product = _productService.GetProductBySku(model.Sku);
            var vendorid = (_workContext.CurrentVendor == null) ? 0 : _workContext.CurrentVendor.Id;
            var fitmentList = _vehicleService.SearchFitments(Sku: model.Sku,vendorId: vendorid);
            var gridModel = new DataSourceResult
            {
                Data = fitmentList.Select(x =>
                {
                    
                    return fitmentList;
                }),
                Total = fitmentList.TotalCount,
            };

            return Json(gridModel);

        }

        public ActionResult GetFitment(int? productID)
        {

            return View();
        }
    }
}
