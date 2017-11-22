using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Fitment;
using Nop.Core;


namespace Nop.Services.Fitment
{
    public interface IFitmentService
    {
        void AddVehicle(VehicleRecord record);
        void AddMake(Make record);
        void AddModel(VehicleModel record);
        // void AddVehicle(VehicleRecord record);
        void AddVehicleType(VehicleType record);
        void AddVehicleTypeGroup(VehicleTypeGroup record);
        void AddEngine(Engine record);
        void AddBaseVehicle(BaseVehicle record);
        IList<VFitment> ListFitments(int? productID);
       // IList<VehicleRecord> ListVehicle(int MakeID = 0, int ModelID = 0, int Year = 1889,int EndYear=1989, int VehicleID = 0);
        IList<BaseVehicle> ListBaseVehicle();
        IList<FYear> GetYears();
        IList<Make> ListMake(int year = 0,int EndYear=0);
        IList<VehicleModel> ListModel(int year = 0,int EndYear=0, int MakeID = 0);
        IList<VehicleRecord> ListVehicle(int year = 0,int EndYear=0, int MakeID = 0, int ModelID = 0);
        VehicleRecord GetVehicle(int VehicleID = 0);
        IList<Engine> ListEngine(VehicleRecord Vehicle);
        VFitment GetFitmentByID(int FitmentID);
        BaseVehicle GetBaseVehicle(int Year = 0, int MakeID=0, int ModelID = 0);
        string GetBaseVehicleName(int BaseVehicleID=0, int Year = 0, int MakeID = 0, int ModelID = 0);
        IList<VFitment> ListFitment(int Year = 0, int MakeID = 0, int ModelID = 0,int BaseVehicelID=0, int VehicleID = 0, int EngineID = 0, int ProductID = 0);
        //string GetBaseVehicleName(int BaseVehicleID);
        IPagedList<VFitment> SearchFitments(
            int pageIndex = 0,
            int pageSize = int.MaxValue,


            int vendorId = 0,
            string Sku = ""
           );
        void AddFitment(VFitment  fitment);
        void UpdateFitment(VFitment fitment);
        void AddFitment(int MakeID = 0, int ModelID = 0, int StartYear = 1889,int EndYear=1889, int VehicleID = 0, int EngineID = 0,int ProductID=0);
        /// <summary>
        /// Logs the specified record.
        /// </summary>
        /// <param name="record">The record.</param>
        //void Log(TrackingRecord record);
    }
}
