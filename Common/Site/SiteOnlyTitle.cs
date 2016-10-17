using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tazeyab.Common
{
    [NotMapped]
    public class SiteOnlyTitle : Site
    {
        public SiteOnlyTitle()
        {
            base.SiteDesc = "";
            //base.SiteLogo = null;
            //base.SiteTags = string.Empty;
            base.SkipType = 0;
        }
    }
}
