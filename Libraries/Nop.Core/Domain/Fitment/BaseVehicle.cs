using System;
using Nop.Core;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Seo;
using Nop.Core.Domain.Security;
using Nop.Core.Domain.Stores;

namespace Nop.Core.Domain.Fitment
{
    public class BaseVehicle:BaseEntity, ILocalizedEntity, ISlugSupported //, IAclSupported, IStoreMappingSupported
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
        public virtual FYear FYear { get; set; }
        public ICollection<VehicleRecord> VehicleRecords { get; set; }

    }
}
