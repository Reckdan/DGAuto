using System;
using Nop.Core;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.Fitment
{
    public class VehicleType:BaseEntity
    {
        public VehicleType()
        {
            this.VehicleModels = new HashSet<VehicleModel>();
        }
      //  public int VehicleTypeID { get; set; }
        public string VehicleTypeName { get; set; }
        public int VehicleTypeGroupID { get; set; }
        public virtual VehicleTypeGroup VehicleTypeGroup { get; set; }
        public virtual ICollection<VehicleModel> VehicleModels { get; set; }
    }
}
