using Nop.Core.Domain.Fitment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Data.Mapping.Fitment
{
    public class FYearMap: NopEntityTypeConfiguration<FYear>
    {
        public FYearMap()
        {
            ToTable("FYear");
            HasKey(m => m.Id);
            Property(m => m.Year);
        }
    }
}
