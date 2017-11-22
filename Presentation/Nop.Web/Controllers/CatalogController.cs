using System;
using System.Linq;
using System.Web.Mvc;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Media;
using Nop.Core.Domain.Vendors;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Security;
using Nop.Services.Stores;
using Nop.Services.Vendors;
using Nop.Web.Factories;
using Nop.Web.Framework.Security;
using Nop.Web.Models.Catalog;
using Nop.Admin.Models.Fitment;
using Nop.Services.Fitment;
using Nop.Core.Domain.Fitment;
using System.Collections.Generic;

namespace Nop.Web.Controllers
{
    public partial class CatalogController : BasePublicController
    {
        #region Fields

        private readonly ICatalogModelFactory _catalogModelFactory;
        private readonly IProductModelFactory _productModelFactory;
        private readonly ICategoryService _categoryService;
        private readonly IManufacturerService _manufacturerService;
        private readonly IProductService _productService;
        private readonly IVendorService _vendorService;
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly IFitmentService _fitmentService;
        private readonly ILocalizationService _localizationService;
        private readonly IWebHelper _webHelper;
        private readonly IProductTagService _productTagService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IAclService _aclService;
        private readonly IStoreMappingService _storeMappingService;
        private readonly IPermissionService _permissionService;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly MediaSettings _mediaSettings;
        private readonly CatalogSettings _catalogSettings;
        private readonly VendorSettings _vendorSettings;

        #endregion

        #region Constructors

        public CatalogController(ICatalogModelFactory catalogModelFactory,
            IProductModelFactory productModelFactory,
            ICategoryService categoryService, 
            IManufacturerService manufacturerService,
            IProductService productService, 
            IVendorService vendorService,
            IWorkContext workContext, 
            IStoreContext storeContext,
            ILocalizationService localizationService,
            IWebHelper webHelper,
            IProductTagService productTagService,
            IGenericAttributeService genericAttributeService,
            IAclService aclService,
            IStoreMappingService storeMappingService,
            IPermissionService permissionService, 
            ICustomerActivityService customerActivityService,
            MediaSettings mediaSettings,
            CatalogSettings catalogSettings,
            VendorSettings vendorSettings,
            IFitmentService fitmentService)
        {
            this._catalogModelFactory = catalogModelFactory;
            this._productModelFactory = productModelFactory;
            this._categoryService = categoryService;
            this._manufacturerService = manufacturerService;
            this._productService = productService;
            this._vendorService = vendorService;
            this._workContext = workContext;
            this._storeContext = storeContext;
            this._localizationService = localizationService;
            this._webHelper = webHelper;
            this._productTagService = productTagService;
            this._genericAttributeService = genericAttributeService;
            this._aclService = aclService;
            this._storeMappingService = storeMappingService;
            this._permissionService = permissionService;
            this._customerActivityService = customerActivityService;
            this._mediaSettings = mediaSettings;
            this._catalogSettings = catalogSettings;
            this._vendorSettings = vendorSettings;
            this._fitmentService = fitmentService;
        }

        #endregion

        #region Categories
        
        [NopHttpsRequirement(SslRequirement.No)]
        public virtual ActionResult Category(int categoryId, CatalogPagingFilteringModel command)
        {
            command.BaseVehicleID = Session["BaseVehicleID"] == null ? 0 : int.Parse(Session["BaseVehicleID"].ToString());
            command.VehicleID = Session["VehicleID"] == null ? 0 : int.Parse(Session["VehicleID"].ToString());
            command.EngineID = Session["EngineID"] == null ? 0 : int.Parse(Session["EngineID"].ToString());
            var category = _categoryService.GetCategoryById(categoryId);
            if (category == null || category.Deleted)
                return InvokeHttp404();

            var notAvailable =
                //published?
                !category.Published ||
                //ACL (access control list) 
                !_aclService.Authorize(category) ||
                //Store mapping
                !_storeMappingService.Authorize(category);
            //Check whether the current user has a "Manage categories" permission (usually a store owner)
            //We should allows him (her) to use "Preview" functionality
            if (notAvailable && !_permissionService.Authorize(StandardPermissionProvider.ManageCategories))
                return InvokeHttp404();

            //'Continue shopping' URL
            _genericAttributeService.SaveAttribute(_workContext.CurrentCustomer, 
                SystemCustomerAttributeNames.LastContinueShoppingPage, 
                _webHelper.GetThisPageUrl(false),
                _storeContext.CurrentStore.Id);

            //display "edit" (manage) link
            if (_permissionService.Authorize(StandardPermissionProvider.AccessAdminPanel) && _permissionService.Authorize(StandardPermissionProvider.ManageCategories))
                DisplayEditLink(Url.Action("Edit", "Category", new { id = category.Id, area = "Admin" }));

            //activity log
            _customerActivityService.InsertActivity("PublicStore.ViewCategory", _localizationService.GetResource("ActivityLog.PublicStore.ViewCategory"), category.Name);

            //model
            var model = _catalogModelFactory.PrepareCategoryModel(category, command);

            //template
            var templateViewPath = _catalogModelFactory.PrepareCategoryTemplateViewPath(category.CategoryTemplateId);
            return View(templateViewPath, model);
        }

        [ChildActionOnly]
        public virtual ActionResult CategoryNavigation(int currentCategoryId, int currentProductId)
        {
            var model = _catalogModelFactory.PrepareCategoryNavigationModel(currentCategoryId, currentProductId);
            return PartialView(model);
        }

        [ChildActionOnly]
        public virtual ActionResult TopMenu()
        {
            var model = _catalogModelFactory.PrepareTopMenuModel();
            return PartialView(model);
        }
        
        [ChildActionOnly]
        public virtual ActionResult HomepageCategories()
        {
            var model = _catalogModelFactory.PrepareHomepageCategoryModels();
            if (!model.Any())
                return Content("");

            return PartialView(model);
        }

        #endregion

        #region Manufacturers

        [NopHttpsRequirement(SslRequirement.No)]
        public virtual ActionResult Manufacturer(int manufacturerId, CatalogPagingFilteringModel command)
        {
            var manufacturer = _manufacturerService.GetManufacturerById(manufacturerId);
            command.BaseVehicleID = Session["BaseVehicleID"] == null ? 0 : int.Parse(Session["BaseVehicleID"].ToString());
            command.VehicleID = Session["VehicleID"] == null ? 0 : int.Parse(Session["VehicleID"].ToString());
            command.EngineID = Session["EngineID"] == null ? 0 : int.Parse(Session["EngineID"].ToString());
            if (manufacturer == null || manufacturer.Deleted)
                return InvokeHttp404();

            var notAvailable =
                //published?
                !manufacturer.Published ||
                //ACL (access control list) 
                !_aclService.Authorize(manufacturer) ||
                //Store mapping
                !_storeMappingService.Authorize(manufacturer);
            //Check whether the current user has a "Manage categories" permission (usually a store owner)
            //We should allows him (her) to use "Preview" functionality
            if (notAvailable && !_permissionService.Authorize(StandardPermissionProvider.ManageManufacturers))
                return InvokeHttp404();

            //'Continue shopping' URL
            _genericAttributeService.SaveAttribute(_workContext.CurrentCustomer, 
                SystemCustomerAttributeNames.LastContinueShoppingPage, 
                _webHelper.GetThisPageUrl(false),
                _storeContext.CurrentStore.Id);
            
            //display "edit" (manage) link
            if (_permissionService.Authorize(StandardPermissionProvider.AccessAdminPanel) && _permissionService.Authorize(StandardPermissionProvider.ManageManufacturers))
                DisplayEditLink(Url.Action("Edit", "Manufacturer", new { id = manufacturer.Id, area = "Admin" }));

            //activity log
            _customerActivityService.InsertActivity("PublicStore.ViewManufacturer", _localizationService.GetResource("ActivityLog.PublicStore.ViewManufacturer"), manufacturer.Name);

            //model
            var model = _catalogModelFactory.PrepareManufacturerModel(manufacturer, command);
            
            //template
            var templateViewPath = _catalogModelFactory.PrepareManufacturerTemplateViewPath(manufacturer.ManufacturerTemplateId);
            return View(templateViewPath, model);
        }

        [NopHttpsRequirement(SslRequirement.No)]
        public virtual ActionResult ManufacturerAll()
        {
            var model = _catalogModelFactory.PrepareManufacturerAllModels();
            return View(model);
        }

        [ChildActionOnly]
        public virtual ActionResult ManufacturerNavigation(int currentManufacturerId)
        {
            if (_catalogSettings.ManufacturersBlockItemsToDisplay == 0)
                return Content("");

            var model = _catalogModelFactory.PrepareManufacturerNavigationModel(currentManufacturerId);

            if (!model.Manufacturers.Any())
                return Content("");
            
            return PartialView(model);
        }

        #endregion

        #region Vendors

        [NopHttpsRequirement(SslRequirement.No)]
        public virtual ActionResult Vendor(int vendorId, CatalogPagingFilteringModel command)
        {
            command.EngineID = Session["Engine"] == null ? 0 : int.Parse(Session["EngineID"].ToString());
            command.VehicleID = Session["VehicleID"] == null ? 0 : int.Parse(Session["VehicleID"].ToString());
            command.BaseVehicleID = Session["EngineID"] == null ? 0 : int.Parse(Session["EngineID"].ToString());
            var vendor = _vendorService.GetVendorById(vendorId);
            if (vendor == null || vendor.Deleted || !vendor.Active)
                return InvokeHttp404();

            //'Continue shopping' URL
            _genericAttributeService.SaveAttribute(_workContext.CurrentCustomer,
                SystemCustomerAttributeNames.LastContinueShoppingPage,
                _webHelper.GetThisPageUrl(false),
                _storeContext.CurrentStore.Id);
            
            //display "edit" (manage) link
            if (_permissionService.Authorize(StandardPermissionProvider.AccessAdminPanel) && _permissionService.Authorize(StandardPermissionProvider.ManageVendors))
                DisplayEditLink(Url.Action("Edit", "Vendor", new { id = vendor.Id, area = "Admin" }));

            //model
            var model = _catalogModelFactory.PrepareVendorModel(vendor, command);

            return View(model);
        }

        [NopHttpsRequirement(SslRequirement.No)]
        public virtual ActionResult VendorAll()
        {
            //we don't allow viewing of vendors if "vendors" block is hidden
            if (_vendorSettings.VendorsBlockItemsToDisplay == 0)
                return RedirectToRoute("HomePage");

            var model = _catalogModelFactory.PrepareVendorAllModels();
            return View(model);
        }

        [ChildActionOnly]
        public virtual ActionResult VendorNavigation()
        {
            if (_vendorSettings.VendorsBlockItemsToDisplay == 0)
                return Content("");

            var model = _catalogModelFactory.PrepareVendorNavigationModel();
            if (!model.Vendors.Any())
                return Content("");
            
            return PartialView(model);
        }

        #endregion

        #region Product tags
        
        [ChildActionOnly]
        public virtual ActionResult PopularProductTags()
        {
            var model = _catalogModelFactory.PreparePopularProductTagsModel();

            if (!model.Tags.Any())
                return Content("");
            
            return PartialView(model);
        }

        [NopHttpsRequirement(SslRequirement.No)]
        public virtual ActionResult ProductsByTag(int productTagId, CatalogPagingFilteringModel command)
        {
            command.BaseVehicleID = Session["BaseVehicleID"] == null ? 0 : int.Parse(Session["BaseVehicleID"].ToString());
            command.VehicleID = Session["VehicleID"] == null ? 0 : int.Parse(Session["VehicleID"].ToString());
            command.EngineID = Session["EngineID"] == null ? 0 : int.Parse(Session["EngineID"].ToString());
            var productTag = _productTagService.GetProductTagById(productTagId);
            if (productTag == null)
                return InvokeHttp404();

            var model = _catalogModelFactory.PrepareProductsByTagModel(productTag, command);
            return View(model);
        }

        [NopHttpsRequirement(SslRequirement.No)]
        public virtual ActionResult ProductTagsAll()
        {
            var model = _catalogModelFactory.PrepareProductTagsAllModel();
            return View(model);
        }

        #endregion

        #region Searching

        [NopHttpsRequirement(SslRequirement.No)]
        [ValidateInput(false)]
        public virtual ActionResult Search(SearchModel model, CatalogPagingFilteringModel command)
        {
            command.BaseVehicleID = Session["BaseVehicleID"] == null ? 0 : int.Parse(Session["BaseVehicleID"].ToString());
            command.VehicleID = Session["VehicleID"]==null?0: int.Parse(Session["VehicleID"].ToString());
            command.EngineID = Session["EngineID"]==null?0: int.Parse(Session["EngineID"].ToString());
            //'Continue shopping' URL
            _genericAttributeService.SaveAttribute(_workContext.CurrentCustomer,
                SystemCustomerAttributeNames.LastContinueShoppingPage,
                _webHelper.GetThisPageUrl(false),
                _storeContext.CurrentStore.Id);

            if (model == null)
                model = new SearchModel();

            model = _catalogModelFactory.PrepareSearchModel(model, command);
            var selList = from ProductCondition e in Enum.GetValues(typeof(ProductCondition))
                          select new
                          {
                              id = (int)e,
                              name = e.ToString()
                          };
            foreach (var data in selList)
            {
                model.Conditions.Add(new SelectListItem { Text = data.name, Value = data.id.ToString(), Selected = data.id == model.ConditionId ? true : false });
            }
            return View(model);
        }
        public virtual ActionResult SearchVehicle()
        {
            var model = new VFitmentModel();

            if(Session["BaseVehicleID"]==null)
                Session.Add("BaseVehicleID", "0");
            
            var years = _fitmentService.GetYears();
            model.YearList.Add(new SelectListItem { Text = "Select Year", Value = "0" });
            foreach (var y in years)
            {
                model.YearList.Add(new SelectListItem { Text = y.Year.ToString(), Value = y.Year.ToString(), Selected = y.Year == 1998 ? true : false });
            }
            int BaseVehicleID = int.Parse(Session["BaseVehicleID"].ToString());
            //int MakeID = int.Parse(Session["BaseVehicleID"].ToString());
            //int ModelID = int.Parse(Session["BaseVehicleID"].ToString());
            string CurrentVehicle = "";
            ViewBag.MessageStatus = "hidden";
            ViewBag.FormStatus = "form-control";
            ViewBag.hform = "0";
            ViewBag.CurrentVehicle = CurrentVehicle;
            if (BaseVehicleID!=0)
            {
                 CurrentVehicle = _fitmentService.GetBaseVehicleName(BaseVehicleID: BaseVehicleID);
                ViewBag.MessageStatus = "alert alert-success";
                ViewBag.CurrentVehicle =CurrentVehicle;
                ViewBag.FormStatus = "hidden";
                ViewBag.hform = "1";
            }
            
            return PartialView("_vehicleSearch",model);
        }
     
        [ChildActionOnly]
        public virtual ActionResult SearchBox()
        {
            var model = _catalogModelFactory.PrepareSearchBoxModel();
            return PartialView(model);
        }

        [ValidateInput(false)]
        public virtual ActionResult SearchTermAutoComplete(string term)
        {
            if (String.IsNullOrWhiteSpace(term) || term.Length < _catalogSettings.ProductSearchTermMinimumLength)
                return Content("");

            //products
            var productNumber = _catalogSettings.ProductSearchAutoCompleteNumberOfProducts > 0 ?
                _catalogSettings.ProductSearchAutoCompleteNumberOfProducts : 10;

            var products = _productService.SearchProducts(
                storeId: _storeContext.CurrentStore.Id,
                keywords: term,
                languageId: _workContext.WorkingLanguage.Id,
                visibleIndividuallyOnly: true,
                pageSize: productNumber);

            var models =  _productModelFactory.PrepareProductOverviewModels(products, false, _catalogSettings.ShowProductImagesInSearchAutoComplete, _mediaSettings.AutoCompleteSearchThumbPictureSize).ToList();
            var result = (from p in models
                          select new
                          {
                              label = p.Name,
                              producturl = Url.RouteUrl("Product", new { SeName = p.SeName }),
                              productpictureurl = p.DefaultPictureModel.ImageUrl
                          })
                          .ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion



        #region Search Vehicle
        //[HttpPost]
        //[AdminAntiForgery(true)]
        [NopHttpsRequirement(SslRequirement.No)]
        public ActionResult ListMake(int year, int EndYear)
        {
            // int mYear = (Fitment == null) ? 0 : Fitment.Year;
            var makes = _fitmentService.ListMake(year: year, EndYear: EndYear);

            List<Select2> makelist = new List<Select2>();
            var distinctMakeList = makes.Distinct().ToList();

            makelist.Add(new Select2 { id = 0, text = "Select Make" });
            foreach (var make in distinctMakeList)
                makelist.Add(new Select2 { id = make.Id, text = make.MakeName });

            return Json(makelist, JsonRequestBehavior.AllowGet);

        }

        //[HttpPost]
        //[AdminAntiForgery(true)]
        [NopHttpsRequirement(SslRequirement.No)]
        public ActionResult ListModel(int year, int EndYear, int MakeID)
        {
            // int mYear = (Fitment == null) ? 0 : Fitment.Year;
            var models = _fitmentService.ListModel(year: year, EndYear: EndYear, MakeID: MakeID);

            List<Select2> modelList = new List<Select2>();
            var distinctModelList = models.Distinct().ToList();

            modelList.Add(new Select2 { id = 0, text = "Select Model" });
            foreach (var model in distinctModelList)
                modelList.Add(new Select2 { id = model.Id, text = model.ModelName });

            return Json(modelList,JsonRequestBehavior.AllowGet);

        }


        //[HttpPost]
        //[AdminAntiForgery(true)]
        [NopHttpsRequirement(SslRequirement.No)]
        public ActionResult ListTrim(int year, int EndYear, int MakeID, int ModelID)
        {
            // int mYear = (Fitment == null) ? 0 : Fitment.Year;
            var vehicles = _fitmentService.ListVehicle(year: year, EndYear: EndYear, MakeID: MakeID, ModelID: ModelID);

            List<Select2> vehiclesList = new List<Select2>();
            var distinctVehicleList = vehicles.Distinct().ToList();

            vehiclesList.Add(new Select2 { id = 0, text = "Select Trim" });
            foreach (var model in distinctVehicleList)
                vehiclesList.Add(new Select2 { id = model.Id, text = model.Trim });

            return Json(vehiclesList,JsonRequestBehavior.AllowGet);

        }


        //[HttpPost]
        //[AdminAntiForgery(true)]
        [NopHttpsRequirement(SslRequirement.No)]
        public ActionResult ListEngine(int VehicelID)
        {
            // int mYear = (Fitment == null) ? 0 : Fitment.Year;
            VehicleRecord vehicle = _fitmentService.GetVehicle(VehicelID);
            var engines = _fitmentService.ListEngine(vehicle);

            List<Select2> engineList = new List<Select2>();
            var distinctEngineList = engines.Distinct().ToList();

            engineList.Add(new Select2 { id = 0, text = "Select Engine" });
            foreach (var model in distinctEngineList)
                engineList.Add(new Select2 { id = model.Id, text = model.EngineDesc });

            return Json(engineList,JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        [NopHttpsRequirement(SslRequirement.No)]
        public ActionResult FindParts(int Year=0, int MakeID=0,int ModelID=0, int VehicleID=0, int EngineID=0 )
        {
            var baseVehicle = _fitmentService.GetBaseVehicle(Year: Year, MakeID: MakeID, ModelID: ModelID);
            Session.Add("BaseVehicleID", baseVehicle.Id);
            Session.Add("VehicleID", VehicleID);
            Session.Add("EngineID", EngineID);
            //SearchModel model = new SearchModel();
            //model.q = "";
            return RedirectToAction("Search", new {@q="" });
            //return Json("Success",JsonRequestBehavior.AllowGet);
        }
        [NopHttpsRequirement(SslRequirement.No)]
        public ActionResult ClearCureentVehicle()
        {
            Session.Add("BaseVehicleID", "0");
            Session.Add("VehicleID", "0");
            Session.Add("EngineID", "0");
            return Json("Success", JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
