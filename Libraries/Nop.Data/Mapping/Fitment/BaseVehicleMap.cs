
using Nop.Core.Domain;
using Nop.Core.Domain.Fitment;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Data.Mapping.Fitment
{
    public class BaseVehicleMap : NopEntityTypeConfiguration<BaseVehicle>
    {
        public BaseVehicleMap()
        {

            ToTable("BaseVehicle");
            HasKey(m => m.Id);
            //Property(m => m.MakeID);
           // Property(m => m.ModelID);
            Property(m => m.Year);
            HasRequired(m => m.Make)
                .WithMany(d => d.BaseVehicles)
                .HasForeignKey(m => m.MakeID)
                .WillCascadeOnDelete(false);

            HasRequired(m => m.VehicleModel)
               .WithMany(d => d.BaseVehicles)
               .HasForeignKey(m => m.ModelID)
               .WillCascadeOnDelete(false);

            HasRequired(m => m.FYear)
              .WithMany(d => d.BaseVehicles)
              .HasForeignKey(m => m.Year)
              .WillCascadeOnDelete(false);



            //Property(m => m.SubModelName);
        }
    }
}

