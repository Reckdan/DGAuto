using Nop.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nop.Web.Models.Fitment
{
    public class FitmentSearchModel : BaseNopEntityModel
    {
        public string SelectVehicle { get; set; }

        public int ProductFit { get; set; }
    }
}