using System;
using Nop.Core;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Extension.Vehicle.Domain
{
    public class VehicleRecord:BaseEntity
    {
        public VehicleRecord()
        {
            this.Engines = new HashSet<Engine>();
        }
       // public int VehicleRecordID { get; set; }

        public string Trim { get; set; }
        public int BaseVehicleID { get; set; }
        public string SubModelName { get; set; }
        public string BodyNumberOfDoors { get; set; }
        public string BodyTypeName { get; set; }
        public virtual BaseVehicle BaseVehicle { get; set; }
        public virtual ICollection<Engine> Engines { get; set; }
    }
}
