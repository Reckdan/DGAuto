using Nop.Web.Framework.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Extension.Vehicle.Infrastructure
{
    class CustomViewEngine:ThemeableRazorViewEngine
    {
        public CustomViewEngine()
        {
            ViewLocationFormats = new[] { "~/plugins/Extension.Vehicle/Views/{0}.cshtml" };
            PartialViewLocationFormats= new[] { "~/plugins/Extension.Vehicle/Views/{0}.cshtml" };
        }
    }
}
