using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.Fitment
{
    public class FYear : BaseEntity
    {
        public FYear()
        {
            this.BaseVehicles = new HashSet<BaseVehicle>();
        }
        public int Year { get; set; }
        public virtual ICollection<BaseVehicle> BaseVehicles { get; set; }
    }
}
