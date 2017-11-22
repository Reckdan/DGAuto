using System;
using Nop.Core;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Catalog;

namespace Nop.Core.Domain.Fitment
{
    public class VehicleRecord:BaseEntity
    {
        private ICollection<Engine> _engines;
        private ICollection<VFitment> _fitments;
        public VehicleRecord()
        {
            this.Engines = new HashSet<Engine>();
        }

      //  private ICollection<Product> _products;
       // public int VehicleRecordID { get; set; }

        public string Trim { get; set; }
        public int BaseVehicleID { get; set; }
        public string SubModelName { get; set; }
        public string BodyNumberOfDoors { get; set; }
        public string BodyTypeName { get; set; }
        public virtual BaseVehicle BaseVehicle { get; set; }
        public virtual ICollection<Engine> Engines
        {
            get
            {
                return this._engines ?? (_engines = new List<Engine>());
            }
            protected set
            {
                this._engines = value;
            }
        }
        public virtual ICollection<VFitment> VFitments
        {
            get
            {
                return this._fitments ?? (_fitments = new List< VFitment > ());
            }protected set
            {
                this._fitments = value;
            }
        }
        //public virtual ICollection<Product> Products
        //{
        //    get
        //    {
        //        return this._products ?? (_products = new List<Product>());
        //    }
        //    protected set
        //    {
        //        this._products = value;
        //    }
        //}
    }
}
