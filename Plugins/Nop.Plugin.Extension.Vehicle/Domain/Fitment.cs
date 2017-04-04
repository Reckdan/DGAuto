using Nop.Core;
using Nop.Core.Domain.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Extension.Vehicle.Domain
{
    public class Fitment:BaseEntity
    {
        public int ProductID { get; set; }
        public string Sku { get; set; }
        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public int Year { get; set; }
        public string Trim { get; set; }
        public string EngineDescription { get; set; }
        //public Product Product { get; set; }
        //public VehicleRecord Vehicle { get; set; }
    }
}
