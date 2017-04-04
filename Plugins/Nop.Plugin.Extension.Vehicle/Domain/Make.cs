using System;
using Nop.Core;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Extension.Vehicle.Domain
{
    public class Make:BaseEntity
    {
        public Make()
        {
            this.BaseVehicles = new HashSet<BaseVehicle>();
        }
        //public int MakeID { get; set; }
        public string MakeName { get; set; }
        public virtual ICollection<BaseVehicle> BaseVehicles { get; set; }
    }
}
