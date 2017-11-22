using Nop.Core;
using Nop.Core.Domain.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.Fitment
{
    public class VFitment:BaseEntity
    {
        public int VehicleRecordID { get; set; }
        public int ProductID { get; set; }
        public int EngineID { get; set; }
        public bool Deleted { get; set; }
        public virtual Product Product { get; set; }
        public virtual VehicleRecord Vehicle { get; set; }
        public virtual Engine Engine { get; set; }
    }
}
