using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mn.NewsCms.Common.BaseClass;
using Mn.NewsCms.Common.Config;
using Mn.NewsCms.Common.Models;

namespace Mn.NewsCms.DomainClasses.WebPartBusiness
{
    public class WebPartManager
    {
        public string getWebParts(string Content, int Width)
        {
            TazehaContext context = new TazehaContext(ServiceFactory.Get<IAppConfigBiz>().ConnectionString());
            string AllStr = string.Empty;
            //var continers = context.WebPartsContainers.Where(x => x.ContainerCode == Content).Select(x => x.WebPartId);
            //var webparts = new List<WebPart>();
            //if (continers != null)
            //    webparts = context.WebParts.Where(x => continers.Concat(x.WebPartId)).ToList();
            var webparts = from x in context.WebParts
                           join y in context.WebPartsContainers on x.WebPartId equals y.WebPartId
                           where y.ContainerCode == Content
                           select x;
            int adscount = webparts.Count();
            if (adscount < 10)
            {
                var webparts2 = context.WebParts.Where(x => x.Global == true).Take(10 - adscount);
                webparts = webparts.Union(webparts2);
            }
            //ads = ads.OrderBy(x => x.AdsType).ToList();
            foreach (var item in webparts)
            {
                string res = "<div class='DWebPartItem'>";
                res += item.HtmlContent;
                res += "</div><br />";
                AllStr += res;
            }
            return AllStr;
        }
    }
}