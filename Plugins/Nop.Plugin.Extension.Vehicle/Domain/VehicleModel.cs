using System;
using Nop.Core;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Extension.Vehicle.Domain
{
    public class VehicleModel:BaseEntity
    {
        public VehicleModel()
        {
            this.BaseVehicles = new HashSet<BaseVehicle>();
        }  
       // public int VehicleModelID { get; set; }
        public string ModelName { get; set; }
        public int MakeID { get; set; }
        public int VehicleTypeID { get; set; }
        public int VehicleTypeGroupID { get; set; }
        public virtual Make Make { get; set; }
        public virtual VehicleType VehicleType { get; set; }
        public virtual ICollection<BaseVehicle> BaseVehicles { get; set; }

    }
}
