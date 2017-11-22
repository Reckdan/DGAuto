using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Data;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Fitment;
using Nop.Data;
using Nop.Services.Customers;
using Nop.Services.Events;
using Nop.Services.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Services.Fitment
{
   public  class FitmentService:IFitmentService
    {
        private const string PRODUCTS_BY_ID_KEY = "Nop.product.id-{0}";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string PRODUCTS_PATTERN_KEY = "Nop.product.";

        private readonly IRepository<VehicleRecord> _vehicleRecordRepo;
        private readonly IRepository<VehicleType> _vehicleTypeRepo;
        private readonly IRepository<VehicleTypeGroup> _vehicleTypeGroupRepo;
        private readonly IRepository<VehicleModel> _modelRepo;
        private readonly IRepository<Make> _makeRepo;
        private readonly IRepository<Engine> _engineRepo;
        private readonly IRepository<BaseVehicle> _baseVehicleRepo;
        private readonly IRepository<VFitment> _vFitmentRepo;
        private readonly IRepository<Product> _product;
        private readonly IRepository<FYear> _year;
        private readonly IDataProvider _dataProvider;
        //private VehicelDBObjectContext _vDBContext;
        private IDbContext _dbcontext;
        private readonly IWorkContext _workContext;
        private readonly IAclService _aclService;
        private readonly CommonSettings _commonSettings;
        private readonly CatalogSettings _catalogSettings;
        private readonly ICacheManager _cacheManager;
        private readonly IEventPublisher _eventPublisher;

        public FitmentService(IRepository<VehicleRecord> vehicleRecord,
            IRepository<VehicleType> vehicleType, IRepository<VehicleTypeGroup> vehicleTypeGroup,
            IRepository<VehicleModel> vehicleModel, IRepository<Make> make, IRepository<Engine> engine,IRepository<FYear> _year,
            IRepository<BaseVehicle> baseVehicle, IDbContext dbContext, IDataProvider dataProvider, IWorkContext workContext,
            IAclService aclService,
            CommonSettings commonSettings,
            CatalogSettings catalogSettings,
            IRepository<VFitment> vFitment, IRepository<Product> product
            ,ICacheManager cacheManager, IEventPublisher eventPublisher)
        {
            _vehicleRecordRepo = vehicleRecord;
            _vehicleTypeRepo = vehicleType;
            _vehicleTypeGroupRepo = vehicleTypeGroup;
            _modelRepo = vehicleModel;
            _makeRepo = make;
            _engineRepo = engine;
            _baseVehicleRepo = baseVehicle;
            _dbcontext = dbContext;
            _dataProvider = dataProvider;
            _workContext = workContext;
            this._year = _year;
            this._vFitmentRepo = vFitment;
            this._aclService = aclService;
            this._commonSettings = commonSettings;
            this._catalogSettings = catalogSettings;
            this._product = product;
            this._eventPublisher = eventPublisher;
            this._cacheManager = cacheManager;
        }

        public void AddMake(Make record)
        {
            _makeRepo.Insert(record);
        }

        public void AddModel(VehicleModel record)
        {
            _modelRepo.Insert(record);
        }

        public void AddVehicle(VehicleRecord record)
        {
            _vehicleRecordRepo.Insert(record);
        }
        public void AddVehicleType(VehicleType record)
        {
            _vehicleTypeRepo.Insert(record);
        }

        public void AddVehicleTypeGroup(VehicleTypeGroup record)
        {
            _vehicleTypeGroupRepo.Insert(record);
        }
        public void AddEngine(Engine record)
        {
            _engineRepo.Insert(record);
        }
        public void AddBaseVehicle(BaseVehicle record)
        {
            _baseVehicleRepo.Insert(record);
        }

        public IList<VFitment> ListFitments(int? productID)
        {
            var ProductIDParameter = _dataProvider.GetParameter();
            ProductIDParameter.ParameterName = "ProductID";
            ProductIDParameter.Value = productID == null ? 0 : productID; //?? string.Empty;
            ProductIDParameter.DbType = DbType.Int32;


            //pass allowed customer role identifiers as comma-delimited string
            var customerRoleIdsParameter = _dataProvider.GetParameter();
            customerRoleIdsParameter.ParameterName = "CustomerRoleIds";
            customerRoleIdsParameter.Value = !_catalogSettings.IgnoreAcl
                ? string.Join(",", _workContext.CurrentCustomer.GetCustomerRoleIds()) : string.Empty;
            customerRoleIdsParameter.DbType = DbType.String;
            List<VFitment> mList = null;
            //invoke stored procedure
            //var fitments = _vDBContext.ExecuteMyProcedureList<Fitment>("GetFitmentForProduct",
            //    ProductIDParameter);
            //return fitments.ToList<Fitment>();
            return mList;

        }

        public IList<VFitment> ListFitment(int Year = 0, int MakeID = 0, int ModelID = 0,int BaseVehicleID=0, int VehicleID = 0, int EngineID = 0, int ProductID = 0)
        {
            if (VehicleID == 0)
            {
                var BaseVQuery = _baseVehicleRepo.Table.Where(b => (b.MakeID == MakeID && b.ModelID == ModelID && b.Year == Year)|| (b.Id==BaseVehicleID && BaseVehicleID!=0));
                var veh = from b in BaseVQuery
                          join v in _vehicleRecordRepo.Table on b.Id equals v.BaseVehicleID
                          select v.Id;
                var fit = from f in _vFitmentRepo.Table
                          join v in veh on f.VehicleRecordID equals v
                          where f.ProductID == ProductID
                          select f;

                return fit.ToList();

                // var fitment=_vFitmentRepo.Table.Where(f=>f.VehicleRecordID)
            }
            else
            {
                var fit = from f in _vFitmentRepo.Table
                          where f.VehicleRecordID == VehicleID && f.ProductID == ProductID
                          select f;
                if (EngineID != 0)
                {
                    fit = fit.Where(f => f.EngineID == EngineID || f.EngineID==0);
                }
                return fit.ToList();
            }
        }

        //public virtual IList<VehicleRecord> ListVehicle(int MakeID=0, int ModelID=0, int Year=1889,int EndYear=1889, int  VehicleID=0)
        //{
        //    if (_commonSettings.UseStoredProceduresIfSupported && _dataProvider.StoredProceduredSupported)
        //    {
        //        var makeParameter = _dataProvider.GetParameter();
        //        makeParameter.ParameterName = "MakeID";
        //        makeParameter.Value = MakeID ;
        //        makeParameter.DbType = DbType.Int32;

        //        var modelParameter = _dataProvider.GetParameter();
        //        modelParameter.ParameterName = "ModelID";
        //        modelParameter.Value = ModelID;
        //        modelParameter.DbType = DbType.Int32;


        //        var yearParameter = _dataProvider.GetParameter();
        //        yearParameter.ParameterName = "Year";
        //        yearParameter.Value = Year;
        //        yearParameter.DbType = DbType.Int32;

        //        var trimParameter = _dataProvider.GetParameter();
        //        trimParameter.ParameterName = "VehicleID";
        //        trimParameter.Value = VehicleID;
        //        trimParameter.DbType = DbType.Int32;

        //        var vehicle = _dbcontext.ExecuteStoredProcedureList<VehicleRecord>("GetVehicle",
        //           makeParameter, modelParameter,
        //           yearParameter, trimParameter);

        //       //i var vehicle=_dbcontext.ex

        //        return vehicle;
        //    }else
        //    {
        //        var query = _vehicleRecordRepo.Table;
        //        query = from q in query
        //                where q.BaseVehicle.MakeID == MakeID && q.BaseVehicle.ModelID == ModelID &&
        //q.BaseVehicle.Year == Year
        //                select q;
        //        if (VehicleID != 0)
        //            query = query.Where(q => q.Id == VehicleID);

        //        return query.ToList<VehicleRecord>();
        //    }
        //}


        public virtual IPagedList<VFitment> SearchFitments(int pageIndex = 0, int pageSize = int.MaxValue, int vendorId = 0, string Sku = "")
        {
            var fitments = new List<VFitment>();
           

            if (_commonSettings.UseStoredProceduresIfSupported && _dataProvider.StoredProceduredSupported)
            {
                //stored procedures are enabled for loading categories and supported by the database. 
                //It's much faster with a large number of categories than the LINQ implementation below 

                //prepare parameters
                var skuParameter = _dataProvider.GetParameter();
                skuParameter.ParameterName = "Sku";
                skuParameter.Value = Sku ?? string.Empty;
                skuParameter.DbType = DbType.String;

                var vendorIDParameter = _dataProvider.GetParameter();
                vendorIDParameter.ParameterName = "VendorID";
                vendorIDParameter.Value = _workContext.CurrentVendor!=null ? _workContext.CurrentVendor.Id : 0;
                vendorIDParameter.DbType = DbType.Int32;

                //pass allowed customer role identifiers as comma-delimited string
                //var customerRoleIdsParameter = _dataProvider.GetParameter();
                //customerRoleIdsParameter.ParameterName = "CustomerRoleIds";
                //customerRoleIdsParameter.Value = !_catalogSettings.IgnoreAcl
                //    ? string.Join(",", _workContext.CurrentCustomer.GetCustomerRoleIds()) : string.Empty;
                //customerRoleIdsParameter.DbType = DbType.String;

                var pageIndexParameter = _dataProvider.GetParameter();
                pageIndexParameter.ParameterName = "PageIndex";
                pageIndexParameter.Value = pageIndex;
                pageIndexParameter.DbType = DbType.Int32;

                var pageSizeParameter = _dataProvider.GetParameter();
                pageSizeParameter.ParameterName = "PageSize";
                pageSizeParameter.Value = pageSize;
                pageSizeParameter.DbType = DbType.Int32;

                var totalRecordsParameter = _dataProvider.GetParameter();
                totalRecordsParameter.ParameterName = "TotalRecords";
                totalRecordsParameter.Direction = ParameterDirection.Output;
                totalRecordsParameter.DbType = DbType.Int32;

                //invoke stored procedure
                var fitment = _dbcontext.ExecuteStoredProcedureList<VFitment>("GetFitmentForProduct",
                    skuParameter, vendorIDParameter,
                    pageIndexParameter, pageSizeParameter, totalRecordsParameter);
                var totalRecords = (totalRecordsParameter.Value != DBNull.Value) ? Convert.ToInt32(totalRecordsParameter.Value) : 0;
               // List<VFitment> pFitment=new List<VFitment>();
                //foreach(var vp in fitment)
                //{
                //    var pf = new VFitmentModel
                //    {
                //        Id=vp.Id,
                //        Sku = vp.Product.Sku,
                //        MakeName = vp.Vehicle.BaseVehicle.Make.MakeName,
                //        ModelName = vp.Vehicle.BaseVehicle.VehicleModel.ModelName,
                //        Year = vp.Vehicle.BaseVehicle.Year,
                //        Trim = vp.Vehicle.Trim,
                //        ProductID = vp.ProductID
                //    };
                //    pFitment.Add(pf);
                //}

                //paging
                return new PagedList<VFitment>(fitment, pageIndex, pageSize, totalRecords);
            }
            else
            {
                var vfitments = new List<VFitment>();
                var query = _vFitmentRepo.Table;


                var vendorList = (from q in query
                                  join p in _product.Table on q.ProductID equals p.Id //where p.VendorId==vendorId
                                  select new
                                  {
                                      q.ProductID,
                                      q.VehicleRecordID,
                                      p.Sku,
                                      p.VendorId

                                  });
                if (vendorId != 0)
                {
                    vendorList = vendorList.Where(v => v.VendorId == vendorId);
                }
                if (Sku != "")
                {
                    vendorList = vendorList.Where(v => v.Sku == Sku);
                }
                var result = (from q in vendorList
                              join v in _vehicleRecordRepo.Table on q.VehicleRecordID equals v.Id into prod_vehicle
                              from pv in prod_vehicle.DefaultIfEmpty()
                              join vb in _baseVehicleRepo.Table on pv.BaseVehicleID equals vb.Id into prod_baseVehicle
                              from p_bv in prod_baseVehicle.DefaultIfEmpty()
                              join m in _modelRepo.Table on p_bv.ModelID equals m.Id into prod_model
                              from p_m in prod_model.DefaultIfEmpty()
                              join d in _makeRepo.Table on p_bv.MakeID equals d.Id into prod_make
                              from p_md in prod_make.DefaultIfEmpty()
                              join e in _engineRepo.Table on p_md.Id equals e.VehicleRecordID into prod_engine
                              from p_e in prod_engine.DefaultIfEmpty()
                              select new
                              {
                                  q.ProductID,
                                  pv.Id,
                                  p_md.MakeName,
                                  p_m.ModelName,
                                  p_bv.Year,
                                  pv.Trim,
                                  p_e.EngineDesc

                              }).ToList();
                //foreach (var rs in result)
                //{
                //    var fitment = new VFitment
                //    {
                //        ProductID = rs.ProductID,
                //        EngineDescription = rs.EngineDesc,
                //        MakeName = rs.MakeName,
                //        ModelName = rs.ModelName,
                //        Trim = rs.Trim,
                //        Year = rs.Year
                //    };
                //   // fitments.Add(fitment);
                //    //fitment.ProductID = rs.ProductID;

                //}
                var fitmentPL = new PagedList<VFitment>(vfitments, pageIndex, pageSize);
                return fitmentPL;
            }
        }

        public void AddFitment(VFitment fitment)
        {
            if (fitment == null)
                throw new ArgumentNullException("fitment");

            //insert
            _vFitmentRepo.Insert(fitment);
        }

        public void AddFitment(int MakeID = 0, int ModelID = 0, int StartYear = 1889, int EndYear = 1889, int VehicleID = 0, int EngineID = 0,int ProductID=0)
        {
            var makeParameter = _dataProvider.GetParameter();
            makeParameter.ParameterName = "MakeID";
            makeParameter.Value = MakeID;
            makeParameter.DbType = DbType.Int32;

            var modelIDParameter = _dataProvider.GetParameter();
            modelIDParameter.ParameterName = "ModelID";
            modelIDParameter.Value = ModelID;
            modelIDParameter.DbType = DbType.Int32;


            var startYearParameter = _dataProvider.GetParameter();
            startYearParameter.ParameterName = "StartYear";
            startYearParameter.Value = StartYear;
            startYearParameter.DbType = DbType.Int32;


            var endYearParameter = _dataProvider.GetParameter();
            endYearParameter.ParameterName = "ModelID";
            endYearParameter.Value = EndYear;
            endYearParameter.DbType = DbType.Int32;

            var VehicleIDParameter = _dataProvider.GetParameter();
            VehicleIDParameter.ParameterName = "VehicleID";
            VehicleIDParameter.Value = VehicleID;
            VehicleIDParameter.DbType = DbType.Int32;

            var EngineIDParameter = _dataProvider.GetParameter();
            EngineIDParameter.ParameterName = "VehicleID";
            EngineIDParameter.Value = VehicleID;
            EngineIDParameter.DbType = DbType.Int32;

            var ProductIDParameter = _dataProvider.GetParameter();
            ProductIDParameter.ParameterName = "ProductID";
            ProductIDParameter.Value = VehicleID;
            ProductIDParameter.DbType = DbType.Int32;

            var fitmentadded = _dbcontext.ExecuteSqlCommand("CreateFitment",false,null,
                 makeParameter, modelIDParameter,startYearParameter,EndYear,VehicleIDParameter,EngineIDParameter,ProductIDParameter);
            if (fitmentadded != 0)
            {
                throw new Exception("Fitment not added. Error during insert");
            }
        }

        public IList<BaseVehicle> ListBaseVehicle()
        {
            var query = _baseVehicleRepo.Table;
            return query.ToList();
        }

        public IList<Make> ListMake(int year = 0,int EndYear=0)
        {
            //var queryBV = _baseVehicleRepo.Table.Where(b => b.Year == year);

            var queryMake = (from m in _makeRepo.Table
                             join b in _baseVehicleRepo.Table on m.Id equals b.MakeID
                             where b.Year>=year
                             select new
                             {
                                 id = m.Id,
                                 MakeName = m.MakeName,
                                 Year = b.Year
                             });
            if (EndYear == 0)
            {
                queryMake = queryMake.Where(q => q.Year == year);
            }
            else
                queryMake = queryMake.Where(q => q.Year >= year && q.Year <= EndYear);

            var Makes = new List<Make>();
            foreach (var m in queryMake)
            {
                var make = new Make
                {
                    Id = m.id,
                    MakeName = m.MakeName
                };
                Makes.Add(make);
            }
            return Makes;
        }

        public IList<VehicleModel> ListModel(int year = 0,int EndYear=0, int MakeID = 0)
        {
            var queryMake = (from m in _modelRepo.Table
                             join b in _baseVehicleRepo.Table on m.Id equals b.ModelID
                             where b.MakeID==MakeID && b.Year>=year
                             select new
                             {
                                 id = m.Id,
                                 ModelName = m.ModelName,
                                 Year=b.Year
                             });
            if (EndYear == 0)
            {
                queryMake = queryMake.Where(q => q.Year == year);
            }
            else
                queryMake = queryMake.Where(q => q.Year >= year && q.Year <= EndYear);
            var vModel = new List<VehicleModel>();
            foreach (var m in queryMake)
            {
                var model = new VehicleModel
                {
                    Id = m.id,
                    ModelName = m.ModelName
                };
                vModel.Add(model);
            }
            return vModel;
        }

        public IList<VehicleRecord> ListVehicle(int year = 0,int EndYear=0, int MakeID = 0, int ModelID = 0)
        {
            var queryVehicle = (from m in _vehicleRecordRepo.Table
                                join b in _baseVehicleRepo.Table on m.BaseVehicleID equals b.Id
                                where b.MakeID == MakeID && b.ModelID == ModelID && b.Year==year
                                select new
                                {
                                    id = m.Id,
                                    trim = m.Trim,
                                    Year=b.Year
                                });
            if (EndYear == 0)
            {
                queryVehicle = queryVehicle.Where(q => q.Year == year);
            }
            else
                queryVehicle = queryVehicle.Where(q => q.Year >= year && q.Year <= EndYear);

            var Vehicles = new List<VehicleRecord>();
            foreach (var m in queryVehicle)
            {
                var vehicle = new VehicleRecord
                {
                    Id = m.id,
                    Trim = m.trim
                };
                Vehicles.Add(vehicle);
            }
            return Vehicles;
        }

        public VehicleRecord GetVehicle(int VehicleID = 0)
        {
            var vehicle = _vehicleRecordRepo.GetById(VehicleID);
            return vehicle;
        }

        public IList<Engine> ListEngine(VehicleRecord Vehicle)
        {
            var engines = _engineRepo.Table.Where(e => e.VehicleRecordID == Vehicle.Id).ToList();
            return engines;
        }

        public IList<FYear> GetYears()
        {
            int tyear = DateTime.Today.Year;
            var years = _year.Table.Where(y=>y.Year<=tyear && y.Year>tyear-25).ToList();
            return years;
        }

        public VFitment GetFitmentByID(int FitmentID)
        {
            VFitment Fitment = _vFitmentRepo.GetById(FitmentID);
            return Fitment;
        }

        public void UpdateFitment(VFitment fitment)
        {
            _vFitmentRepo.Update(fitment);

            

            //cache
            _cacheManager.RemoveByPattern(PRODUCTS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(fitment.Product);
        }

        public BaseVehicle GetBaseVehicle(int Year = 0, int MakeID = 0, int ModelID = 0)
        {
            var bV = _baseVehicleRepo.Table.Where(y => y.Year == Year && y.MakeID == MakeID && y.ModelID == ModelID).FirstOrDefault();
            return bV;
        }

        public string GetBaseVehicleName(int BaseVehicleID=0, int Year = 0, int MakeID = 0, int ModelID = 0)
        {

            var bV = new BaseVehicle();

            if (BaseVehicleID == 0)
                bV = _baseVehicleRepo.Table.Where(y => y.Year == Year && y.MakeID == MakeID && y.ModelID == ModelID).FirstOrDefault();
            else
                bV = _baseVehicleRepo.GetById(BaseVehicleID);

            string baseVehicleName =bV==null?"": bV.Year.ToString() + " " + bV.Make.MakeName + " " + bV.VehicleModel.ModelName;
            return baseVehicleName;
        }
    }
}

