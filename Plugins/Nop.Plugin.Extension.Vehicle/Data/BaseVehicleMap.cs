using Nop.Plugin.Extension.Vehicle.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Extension.Vehicle.Data
{
    public class BaseVehicleMap : EntityTypeConfiguration<BaseVehicle>
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

            //Property(m => m.SubModelName);
        }
    }
}

