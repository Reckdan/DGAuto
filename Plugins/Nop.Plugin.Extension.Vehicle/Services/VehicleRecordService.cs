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
            CatalogSettings catalogSettings)
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
            this._aclService = aclService;
            this._commonSettings = commonSettings;
            this._catalogSettings = catalogSettings;

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
    }
}
