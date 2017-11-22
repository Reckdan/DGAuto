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
    public class VFitmentMap: NopEntityTypeConfiguration<VFitment>
    {
        public VFitmentMap()
        {
            ToTable("VFitment");
            HasKey(m => m.Id);
            // Property(m => m.Deleted);
            //Property(m => m.ProductID);
            // Property(m => m.VehicleRecordID);
            // Property(m => m.EngineID);
            Property(m => m.Deleted);
            this.HasRequired(pm => pm.Vehicle)
               .WithMany(p => p.VFitments)
               .HasForeignKey(pm => pm.VehicleRecordID);

            this.HasRequired(pm => pm.Engine)
               .WithMany(p => p.VFitments)
               .HasForeignKey(pm => pm.EngineID);

            this.HasRequired(pm => pm.Product)
               .WithMany(p => p.VFitments)
               .HasForeignKey(pm => pm.ProductID);
        }
        
    }
}
