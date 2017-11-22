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
    public class VehicleModelMap: NopEntityTypeConfiguration<VehicleModel>
    {
        public VehicleModelMap()
        {

            ToTable("VehicleModel");
            HasKey(m => m.Id);
          //  Property(m => m.MakeID);
            Property(m => m.ModelName).HasMaxLength(125);
          //  Property(m => m.VehicleTypeID);
            Property(m => m.VehicleTypeGroupID);

            HasRequired(m => m.VehicleType)
               .WithMany(d => d.VehicleModels)
               .HasForeignKey(m => m.VehicleTypeID)
               .WillCascadeOnDelete(false); 

            //HasRequired(m => m.VehicleTypeGroupID)
            //   .WithMany(d => d.VehicleModels)
            //   .HasForeignKey(m => m.VehicleTypeID);


        }
    }
}
