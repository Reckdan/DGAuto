using Nop.Core.Domain.Fitment;
//using Nop.Plugin.Extension.Vehicle.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Data.Mapping.Fitment
{
    public class VehicleRecordMap: NopEntityTypeConfiguration<VehicleRecord>
    {
        public VehicleRecordMap()
        {
            ToTable("Vehicle");
            HasKey(m => m.Id);
          //  Property(m => m.BaseVehicleID);
            Property(m => m.BodyNumberOfDoors);
            Property(m => m.BodyTypeName);
            Property(m => m.SubModelName);

            HasRequired(m => m.BaseVehicle)
               .WithMany(d => d.VehicleRecords)
               .HasForeignKey(m => m.BaseVehicleID)
               .WillCascadeOnDelete(false); 

        }
    }
}
