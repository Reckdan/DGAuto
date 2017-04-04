using Nop.Plugin.Extension.Vehicle.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Extension.Vehicle.Data
{
    public class VehicleTypeGroupMap:EntityTypeConfiguration<VehicleTypeGroup>
    {
        public VehicleTypeGroupMap()
        {

            ToTable("VehicleTypeGroup");
            HasKey(m => m.Id);
            Property(m => m.VehicleTypeGroupName);
           

            //Property(m => m.BodyNumberOfDoors);
            //Property(m => m.BodyTypeName);
            //Property(m => m.SubModelName);
        }
    }
}
