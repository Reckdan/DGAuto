using System;
using Nop.Core;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Extension.Vehicle.Domain
{
    public class BaseVehicle:BaseEntity
    {
        public BaseVehicle()
        {
            this.VehicleRecords = new HashSet<VehicleRecord>();
        }
        
        public int MakeID { get; set; }
        public int ModelID { get; set; }
        public int Year { get; set; }
        public virtual Make Make { get; set; }
        public virtual VehicleModel VehicleModel { get; set; }
        public ICollection<VehicleRecord> VehicleRecords { get; set; }

    }
}
