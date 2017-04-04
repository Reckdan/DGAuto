using System;
using Nop.Core;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Extension.Vehicle.Domain
{
    public class VehicleTypeGroup:BaseEntity
    {
        //public int VehicleTypeGroupID { get; set; }
        public string VehicleTypeGroupName { get; set; }
        public virtual ICollection<VehicleType> VehicleTypes { get; set; }
    }
}
