using Nop.Plugin.Extension.Vehicle.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Extension.Vehicle.Data
{
    public class EngineMap:EntityTypeConfiguration<Engine>
    {
        public EngineMap()
        {

            ToTable("Engine");
            HasKey(m => m.Id);
            Property(m => m.EngineDesc);
            Property(m => m.CC);
            Property(m => m.CID);
            Property(m => m.AspirationNamae);
            Property(m => m.CylinderHeadTypeName);
            Property(m => m.Cylinders);
            Property(m => m.BlockType);
            Property(m => m.FuelName);
           // Property(m => m.VehicleID);
            Property(m => m.Liter);

            HasRequired(m => m.VehicleRecord)
               .WithMany(d => d.Engines)
               .HasForeignKey(m => m.VehicleRecordID)
               .WillCascadeOnDelete(false); ;
        }
    }
}
