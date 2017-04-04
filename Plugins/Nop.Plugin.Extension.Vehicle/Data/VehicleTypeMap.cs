using Nop.Plugin.Extension.Vehicle.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Extension.Vehicle.Data
{
    class VehicleTypeMap:EntityTypeConfiguration<VehicleType>
    {
        public VehicleTypeMap()
        {

            ToTable("VehicleType");
            HasKey(m => m.Id);
            Property(m => m.VehicleTypeName);
            Property(m => m.VehicleTypeGroupID);
            HasRequired(m => m.VehicleTypeGroup)
                 .WithMany(d => d.VehicleTypes)
                 .HasForeignKey(m => m.VehicleTypeGroupID)
                 .WillCascadeOnDelete(false); 

        }

    }
}
