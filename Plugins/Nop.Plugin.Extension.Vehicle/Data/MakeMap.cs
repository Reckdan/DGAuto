using Nop.Plugin.Extension.Vehicle.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Extension.Vehicle.Data
{
    public class MakeMap:EntityTypeConfiguration<Make>
    {
        public MakeMap()
        {

            ToTable("Make");
            HasKey(m => m.Id);
            Property(m => m.MakeName);
      
        }
    }
}
