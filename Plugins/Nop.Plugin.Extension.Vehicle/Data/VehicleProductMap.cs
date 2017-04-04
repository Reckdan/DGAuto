using Nop.Plugin.Extension.Vehicle.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Extension.Vehicle.Data
{
    public class VehicleProductMap: EntityTypeConfiguration<VehicleProduct>
    {
        public VehicleProductMap()
        {
            ToTable("VehicleProduct");
            HasKey(m => m.Id);
            Property(m => m.ProductID);
            Property(m => m.VehicleRecordID);
        }
        
    }
}
