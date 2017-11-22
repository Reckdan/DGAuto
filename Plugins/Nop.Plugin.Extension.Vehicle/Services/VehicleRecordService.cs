using Nop.Core;
using Nop.Core.Data;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Common;
using Nop.Plugin.Extension.Vehicle.Data;
using Nop.Plugin.Extension.Vehicle.Domain;
using Nop.Services.Customers;
using Nop.Services.Security;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Extension.Vehicle.Services
{
    public interface IVehicleRecordService
    {
        void AddVehicle(VehicleRecord record);
        void AddMake(Make record);
        void AddModel(VehicleModel record);
       // void AddVehicle(VehicleRecord record);
        void AddVehicleType(VehicleType record);
        void AddVehicleTypeGroup(VehicleTypeGroup record);
        void AddEngine(Engine record);
        void AddBaseVehicle(BaseVehicle record);
        List<Fitment> GetFitments(int? productID);
        IPagedList<Fitment> SearchFitments(
            int pageIndex = 0,
            int pageSize = int.MaxValue,
          
            
            int vendorId = 0,
            string Sku = ""
           );
        /// <summary>
        /// Logs the specified record.
        /// </summary>
        /// <param name="record">The record.</param>
        //void Log(TrackingRecord record);
    }
    public class VehicleRecordService : IVehicleRecordService
    {
        private readonly IRepository<VehicleRecord> _vehicleRecordRepo;
        private readonly IRepository<VehicleType> _vehicleTypeRepo;
        private readonly IRepository<VehicleTypeGroup> _vehicleTypeGroupRepo;
        private readonly IRepository<VehicleModel> _modelRepo;
        private readonly IRepository<Make> _makeRepo;
        private readonly IRepository<Engine> _engineRepo;
        private readonly IRepository<BaseVehicle> _baseVehicleRepo;
        private readonly IRepository<VehicleProduct> _vehicleProductRepo;
        private readonly IRepository<Product> _product;
        private readonly IDataProvider _dataProvider;
        private VehicelDBObjectContext _vDBContext;
        private readonly IWorkContext _workContext;
        private readonly IAclService _aclService;
        private readonly CommonSettings _commonSettings;
        private readonly CatalogSettings _catalogSettings;

        public VehicleRecordService(IRepository<VehicleRecord> vehicleRecord,
            IRepository<VehicleType> vehicleType, IRepository<VehicleTypeGroup> vehicleTypeGroup,
            IRepository<VehicleModel> vehicleModel, IRepository<Make> make, IRepository<Engine> engine,
            IRepository<BaseVehicle> baseVehicle,VehicelDBObjectContext vDBcontext,IDataProvider dataProvider,IWorkContext workContext,
            IAclService aclService,
            CommonSettings commonSettings,
            CatalogSettings catalogSettings,
            IRepository<VehicleProduct> vehicleproduct,IRepository<Product> product)
        {
            _vehicleRecordRepo = vehicleRecord;
            _vehicleTypeRepo = vehicleType;
            _vehicleTypeGroupRepo = vehicleTypeGroup;
            _modelRepo = vehicleModel;
            _makeRepo = make;
            _engineRepo = engine;
            _baseVehicleRepo = baseVehicle;
            _vDBContext = vDBcontext;
            _dataProvider = dataProvider;
            _workContext = workContext;
            this._vehicleProductRepo = vehicleproduct;
            this._aclService = aclService;
            this._commonSettings = commonSettings;
            this._catalogSettings = catalogSettings;
            this._product = product;
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

        public List<Fitment> GetFitments(int? productID)
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
            List<Fitment> mList = null;
            //invoke stored procedure
            //var fitments = _vDBContext.ExecuteMyProcedureList<Fitment>("GetFitmentForProduct",
            //    ProductIDParameter);
            //return fitments.ToList<Fitment>();
            return mList;
            
        }

        public virtual IPagedList<Fitment> SearchFitments(int pageIndex = 0, int pageSize = int.MaxValue, int vendorId = 0, string Sku = "")
        {
            var fitments = new List<Fitment>();
            var query = _vehicleProductRepo.Table;
            //if (Sku != "")
            //{
            //    query=from q in query wher
            //}

            var vendorList= (from q in query 
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
            foreach(var rs in result)
            {
                var fitment = new Fitment
                {
                    ProductID = rs.ProductID,
                    EngineDescription = rs.EngineDesc,
                    MakeName = rs.MakeName,
                    ModelName = rs.ModelName,
                    Trim = rs.Trim,
                    Year = rs.Year
                };
                fitments.Add(fitment);
                //fitment.ProductID = rs.ProductID;

            }
            var fitmentPL = new PagedList<Fitment>(fitments, pageIndex, pageSize);
            return fitmentPL;
        }
    }
}
