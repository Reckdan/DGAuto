using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Fitment;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Nop.Admin.Models.Fitment
{
    public partial class VFitmentModel : BaseNopEntityModel
    {
        public VFitmentModel()
        {
            YearList = new List<SelectListItem>();
            MakeList = new List<SelectListItem>();
            ModelList = new List<SelectListItem>();
            TrimList = new List<SelectListItem>();
            EngineList = new List<SelectListItem>();
            //if (PageSize < 1)
            //{
            //    PageSize = 5;
            //}
        }
        public int ProductID { get; set; }
        [NopResourceDisplayName("Admin.Catalog.Products.Fields.Sku")]
        public string Sku { get; set; }
        [NopResourceDisplayName("Admin.Fitment.Makes.Fields.Make")]
        public string MakeName { get; set; }
        [NopResourceDisplayName("Admin.Fitment.VehicleModels.Fields.Model")]
        public string ModelName { get; set; }
        [NopResourceDisplayName("Admin.Fitment.BaseVehicle.Fields.Year")]
        public int Year { get; set; }
        [NopResourceDisplayName("Admin.Fitment.Vehicles.Fields.Trim")]
        public string Trim { get; set; }
        [NopResourceDisplayName("Admin.Fitment.Engines.Fields.Engine")]
        public string EngineDescription { get; set; }

        [NopResourceDisplayName("Admin.Fitment.Engines.Fields.EndYear")]
        public int EndYear { get; set; }
        [NopResourceDisplayName("Admin.Fitment.Makes.Fields.Make")]
        public int MakeID { get; set; }
        [NopResourceDisplayName("Admin.Fitment.VehicleModels.Fields.Model")]
        public int ModelID { get; set; }
        [NopResourceDisplayName("Admin.Fitment.Engines.Fields.Engine")]
        public int EngineID { get; set; }
        [NopResourceDisplayName("Admin.Fitment.Engines.Fields.Vehicle")]
        public int VehicleID { get; set; }

        public List<SelectListItem> YearList { get; set; }
        public List<SelectListItem> MakeList
        {
            get; set;
            //public Product Product { get; set; }
            //public VehicleRecord Vehicle { get; set; }
        }

        public List<SelectListItem> ModelList { get; set; }
        public List<SelectListItem> TrimList { get; set; }
        public List<SelectListItem> EngineList { get; set; }
    }

        public class Select2 : BaseNopModel
        {
            public int id { get; set; }
            public string text { get; set; }
        }
    
}
