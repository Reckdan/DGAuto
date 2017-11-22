using System;
using Nop.Core;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.Fitment
{
    public class Engine:BaseEntity
    {
        private ICollection<VFitment> _fitments;
        public int VehicleRecordID { get; set; }
        public string EngineDesc { get; set; }
        public string Liter { get; set; }
        public string CC { get; set; }
        public string CID { get; set; }
        public string BlockType { get; set; }
        public string Cylinders { get; set; }
        public string FuelName { get; set; }
        public string CylinderHeadTypeName { get; set; }
        public string AspirationName { get; set; }
        public virtual VehicleRecord VehicleRecord { get; set; }
        public virtual ICollection<VFitment> VFitments
        {
            get
            {
                return this._fitments ?? (_fitments = new List<VFitment>());
            }
            protected set
            {
                this._fitments = value;
            }
        }
    }
}
