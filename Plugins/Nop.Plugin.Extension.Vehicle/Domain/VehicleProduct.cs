using Nop.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Extension.Vehicle.Domain
{
    public class VehicleProduct:BaseEntity
    {
        public int VehicleRecordID { get; set; }
        public int ProductID { get; set; }
    }
}
