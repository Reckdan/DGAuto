using Nop.Core;
using Nop.Core.Data;

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
using Nop.Admin.Controllers;
using Nop.Services.Fitment;
using Nop.Core.Domain.Fitment;
using Nop.Admin.Models.Fitment;
using Nop.Admin.Models.Catalog;
using Nop.Admin.Extensions;
using Nop.Services.Logging;
using Nop.Services.Localization;
using Nop.Web.Framework.Security;
using Nop.Services.Security;

namespace Nop.Admin.Controllers
{
    public class FitmentController : BaseAdminController
    {
        private readonly ICustomerActivityService _customerActivityService;
        private readonly IFitmentService _fitmentService;
        //private IRepository<VehicelDBObjectContext> _context;
        private readonly IProductService _productService;
        //  private IRepository<VehicleRecord> _vehicleRecord;
        private readonly IWorkContext _workContext;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;

        public FitmentController(IWorkContext workContext, IFitmentService fitmentService,
            IProductService productService,ILocalizationService localizationService,
            ICustomerActivityService customerActivityService, IPermissionService permissionService)
        {
            _fitmentService = fitmentService;
            _workContext = workContext;
            _productService = productService;
            _localizationService = localizationService;
            _customerActivityService = customerActivityService;
            _permissionService = permissionService;

        }
        [HttpGet]
        public ActionResult CreateFitment()
        {
            //var basevehicle = _fitmentService.GetBaseVehicle();
            var model = new VFitmentModel();
            var years = _fitmentService.GetYears();
            model.YearList.Add(new SelectListItem { Text = "Select Year", Value = "0" });
            foreach (var y in years)
            {
                model.YearList.Add(new SelectListItem { Text = y.Year.ToString(), Value = y.Year.ToString(), Selected = y.Year == 1998 ? true:false });
            }
            return PartialView("_CreateFitment",model);
        }


        public ActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            var fitment = _fitmentService.GetFitmentByID(id);

            if (fitment == null || fitment.Deleted)
                //No product found with the specified id
                return RedirectToAction("Index");

            //a vendor should have access only to his products
            var year = fitment.Vehicle.BaseVehicle.Year;
            var MakeID = fitment.Vehicle.BaseVehicle.MakeID;
            var ModelID = fitment.Vehicle.BaseVehicle.ModelID;
            var VehicleID = fitment.VehicleRecordID;
            var EngineID = fitment.EngineID;


            if (_workContext.CurrentVendor != null && fitment.Product.VendorId != _workContext.CurrentVendor.Id)
                return RedirectToAction("Index");
            var years = _fitmentService.GetYears();
            var model = new VFitmentModel { Id = id };
            model.Sku = fitment.Product.Sku;
            model.Year = year;
            model.MakeID = MakeID;
            model.ModelID = ModelID;
            model.VehicleID = VehicleID;
            model.EngineID = EngineID;
            model.YearList.Add(new SelectListItem { Text = "Select Year", Value = "0" });
            foreach (var y in years)
            {
                model.YearList.Add(new SelectListItem { Text = y.Year.ToString(), Value = y.Year.ToString(), Selected=(year==y.Year) });
            }

            var makelist = _fitmentService.ListMake(year: year);
            model.YearList.Add(new SelectListItem { Text = "All", Value = "0" });
            foreach (var make in makelist)
            {
                model.MakeList.Add(new SelectListItem { Text = make.MakeName.ToString(), Value = make.Id.ToString(), Selected = (MakeID == make.Id) });
            }

            var modelList = _fitmentService.ListModel(year: year, MakeID: MakeID);
            model.ModelList.Add(new SelectListItem (){ Text = "All", Value = "0" });
            foreach (var vmodel in modelList)
            {
                model.ModelList.Add(new SelectListItem { Text = vmodel.ModelName.ToString(), Value = vmodel.Id.ToString(), Selected = (ModelID == vmodel.Id) });
            }

            var trimList = _fitmentService.ListVehicle(year: year, MakeID: MakeID, ModelID: ModelID);
            model.TrimList.Add(new SelectListItem { Text = "All", Value = "0" });
            foreach (var trim in trimList)
            {
                model.TrimList.Add(new SelectListItem { Text = trim.Trim.ToString(), Value = trim.Id.ToString(), Selected = (VehicleID == trim.Id) });
            }

            var engineList = _fitmentService.ListEngine(fitment.Vehicle);
            model.EngineList.Add(new SelectListItem { Text = "All", Value = "0" });
            foreach (var engine in engineList)
            {
                model.EngineList.Add(new SelectListItem { Text = engine.EngineDesc.ToString(), Value = engine.Id.ToString(), Selected = (EngineID == engine.Id) });
            }

            return View(model);

        }
        [HttpPost]
        public ActionResult Edit(VFitmentModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();
            if (ModelState.IsValid) { 
            var fitment = new VFitment { ProductID = model.ProductID, VehicleRecordID = model.VehicleID, EngineID = model.EngineID };
            _fitmentService.UpdateFitment(fitment);

           

            //activity log
            _customerActivityService.InsertActivity("EditFitment", _localizationService.GetResource("ActivityLog.EditFitment"), model.Sku);

            SuccessNotification(_localizationService.GetResource("Admin.Catalog.Categories.Updated"));
                return RedirectToAction("Index");
            }
            var mfitment = _fitmentService.GetFitmentByID(model.Id);

            if (mfitment == null || mfitment.Deleted)
                //No product found with the specified id
                return RedirectToAction("Index");

            //a vendor should have access only to his products
            var year = mfitment.Vehicle.BaseVehicle.Year;
            var MakeID = mfitment.Vehicle.BaseVehicle.MakeID;
            var ModelID = mfitment.Vehicle.BaseVehicle.ModelID;
            var VehicleID = mfitment.VehicleRecordID;
            var EngineID = mfitment.EngineID;
            var years = _fitmentService.GetYears();
            model.YearList.Add(new SelectListItem { Text = "Select Year", Value = "0" });
            foreach (var y in years)
            {
                model.YearList.Add(new SelectListItem { Text = y.Year.ToString(), Value = y.Year.ToString(), Selected = (year == y.Year) });
            }

            var makelist = _fitmentService.ListMake(year: year);
            model.YearList.Add(new SelectListItem { Text = "All", Value = "0" });
            foreach (var make in makelist)
            {
                model.MakeList.Add(new SelectListItem { Text = make.MakeName.ToString(), Value = make.Id.ToString(), Selected = (MakeID == make.Id) });
            }

            var modelList = _fitmentService.ListModel(year: year, MakeID: MakeID);
            model.ModelList.Add(new SelectListItem() { Text = "All", Value = "0" });
            foreach (var vmodel in modelList)
            {
                model.ModelList.Add(new SelectListItem { Text = vmodel.ModelName.ToString(), Value = vmodel.Id.ToString(), Selected = (ModelID == vmodel.Id) });
            }

            var trimList = _fitmentService.ListVehicle(year: year, MakeID: MakeID, ModelID: ModelID);
            model.TrimList.Add(new SelectListItem { Text = "All", Value = "0" });
            foreach (var trim in trimList)
            {
                model.TrimList.Add(new SelectListItem { Text = trim.Trim.ToString(), Value = trim.Id.ToString(), Selected = (VehicleID == trim.Id) });
            }

            var engineList = _fitmentService.ListEngine(mfitment.Vehicle);
            model.EngineList.Add(new SelectListItem { Text = "All", Value = "0" });
            foreach (var engine in engineList)
            {
                model.EngineList.Add(new SelectListItem { Text = engine.EngineDesc.ToString(), Value = engine.Id.ToString(), Selected = (EngineID == engine.Id) });
            }

            return View(model);


        }

        [HttpPost]
        public ActionResult SaveFitment([Bind(Include = "Year, EndYear, Sku, MakeID, ModelID, VehicleID, EngineID")]VFitmentModel Fitment)
        {
            var product = _productService.GetProductBySku(Fitment.Sku);
            if (product == null)
            {
                ErrorNotification("The Sku entered is not valid", false);
                return Json(new { Result = true });
            }
            if (Fitment.EndYear == 0)

            {
                if (Fitment.VehicleID == 0)
                {
                   

                    var vehicles = _fitmentService.ListVehicle(MakeID: Fitment.MakeID,
                        ModelID: Fitment.ModelID, year: Fitment.Year);
                    var vfits = new List<VFitment>();
                    foreach (var vehicle in vehicles)
                    {
                        var fitment = new VFitment
                        {
                            ProductID = product.Id,
                            VehicleRecordID = vehicle.Id,
                            EngineID = Fitment.EngineID



                        };
                        _fitmentService.AddFitment(fitment);
                    }
                }else
                {
                    // var vehicle = _fitmentService.GetVehicle(Fitment.VehicleID);
                    var fitment = new VFitment
                    {
                        ProductID = product.Id,
                        VehicleRecordID = Fitment.VehicleID,
                        EngineID = Fitment.EngineID

                    };
                    _fitmentService.AddFitment(fitment);
                }
            }
            else
            {
                _fitmentService.AddFitment(MakeID: Fitment.MakeID,
                    ModelID: Fitment.ModelID, StartYear: Fitment.Year, EndYear: Fitment.EndYear, VehicleID: Fitment.VehicleID,
                    EngineID: Fitment.EngineID,ProductID:product.Id);
                
            }

            //activity log
            _customerActivityService.InsertActivity("DeleteProduct", _localizationService.GetResource("ActivityLog.AddNewProduct"), product.Sku);

            SuccessNotification(_localizationService.GetResource("Admin.Fitment.Added"));
            //return RedirectToAction("List");
            return Json(new { Result = true });


        }
        [HttpPost]
        [AdminAntiForgery(true)]
        public ActionResult ListMake(int year, int EndYear)
        {
            // int mYear = (Fitment == null) ? 0 : Fitment.Year;
            var makes = _fitmentService.ListMake(year: year,EndYear: EndYear);
            
            List<Select2> makelist = new List<Models.Fitment.Select2>();
            var distinctMakeList = makes.Distinct().ToList();

            makelist.Add(new Select2 { id = 0, text = "Select Make...." });
            foreach (var make in distinctMakeList)
                makelist.Add(new Select2 { id = make.Id, text = make.MakeName });

            return Json(makelist);    

        }

        [HttpPost]
        [AdminAntiForgery(true)]
        public ActionResult ListModel(int year,int EndYear,int MakeID)
        {
            // int mYear = (Fitment == null) ? 0 : Fitment.Year;
            var models = _fitmentService.ListModel(year: year, EndYear: EndYear, MakeID: MakeID);

            List<Select2> modelList = new List<Models.Fitment.Select2>();
            var distinctModelList = models.Distinct().ToList();

            modelList.Add(new Select2 { id = 0, text = "All" });
            foreach (var model in distinctModelList)
                modelList.Add(new Select2 { id = model.Id, text = model.ModelName });

            return Json(modelList);

        }


        [HttpPost]
        [AdminAntiForgery(true)]
        public ActionResult ListTrim(int year,int EndYear, int MakeID, int ModelID)
        {
            // int mYear = (Fitment == null) ? 0 : Fitment.Year;
            var vehicles = _fitmentService.ListVehicle(year: year,EndYear: EndYear, MakeID: MakeID,ModelID: ModelID);

            List<Select2> vehiclesList = new List<Models.Fitment.Select2>();
            var distinctVehicleList = vehicles.Distinct().ToList();

            vehiclesList.Add(new Select2 { id = 0, text = "All" });
            foreach (var model in distinctVehicleList)
                vehiclesList.Add(new Select2 { id = model.Id, text = model.Trim });

            return Json(vehiclesList);

        }


        [HttpPost]
        [AdminAntiForgery(true)]
        public ActionResult ListEngine(int VehicelID)
        {
            // int mYear = (Fitment == null) ? 0 : Fitment.Year;
            VehicleRecord vehicle = _fitmentService.GetVehicle(VehicelID);
            var engines = _fitmentService.ListEngine(vehicle);

            List<Select2> engineList = new List<Models.Fitment.Select2>();
            var distinctEngineList = engines.Distinct().ToList();

            engineList.Add(new Select2 { id=0,text="All"});
            foreach (var model in distinctEngineList)
                engineList.Add(new Select2 { id = model.Id, text = model.EngineDesc });
            
            return Json(engineList);

        }


        public ActionResult Index()
        {
            //var basevehicle = _fitmentService.GetBaseVehicle();
            var model = new VFitmentModel();
           // model.Sku = "Testing";
            var years = _fitmentService.GetYears();
            model.YearList.Add(new SelectListItem { Text = "Select Year", Value = "0" });
            foreach (var y in years)
            {
                model.YearList.Add(new SelectListItem { Text = y.Year.ToString(), Value = y.Year.ToString() });
            }
            return View(model);
        }

        public ActionResult FitmentList(DataSourceRequest command, ProductListModel model)
        {
            //var product = _productService.GetProductBySku(model.Sku);
            var vendorid = (_workContext.CurrentVendor == null) ? 0 : _workContext.CurrentVendor.Id;
            var fitmentList = _fitmentService.SearchFitments(Sku: model.GoDirectlyToSku,vendorId: vendorid);
            // Nop.Core.IPagedList stat;
            var pFitment = new List<VFitmentModel>();
            foreach (var vp in fitmentList)
            {
               
                var pf = new VFitmentModel
                {
                    Id = vp.Id,
                    Sku = vp.Product.Sku,
                    MakeName = vp.Vehicle.BaseVehicle.Make.MakeName,
                    ModelName = vp.Vehicle.BaseVehicle.VehicleModel.ModelName,
                    Year = vp.Vehicle.BaseVehicle.Year,
                    Trim = vp.Vehicle.Trim,
                    ProductID = vp.ProductID
                };
                pFitment.Add(pf);
            }
            var gridModel = new DataSourceResult
            {
                Data = pFitment,
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
