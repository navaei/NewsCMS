using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tazeyab.Common.Models;

namespace Tazeyab.DomainClasses.ContentManagment
{
    public class AdsManager
    {
        public string getAds(string Content, int Width)
        {
            TazehaContext context = new TazehaContext();
            string allAds = string.Empty;
            //var continer = context.AdsContainers.SingleOrDefault(x => x.ContainerCode == Content);
            //var ads = new List<Ad>();
            //if (continer != null)
            //    ads = context.Ads.Where(x => x.AdsId == continer.AdsId).ToList();
            var ads = (from x in context.Ads
                       join y in context.AdsContainers on x.AdsId equals y.AdsId
                       where y.ContainerCode == Content && x.Disable != true
                       select x).ToList();
            int adscount = ads.Count();
            if (adscount < 10)
            {
                var ads2 = context.Ads.Where(x => x.Global == true && (!x.Disable.HasValue || x.Disable.Value != true)).Take(10 - adscount).ToList();
                ads.AddRange(ads2);
            }
            ads = ads.OrderBy(x => x.AdsType).ToList();
            foreach (var item in ads)
            {
                string res = "<div class='DAdsItem'>";
                if (item.AdsType == 1)
                {
                    //---------Image--------
                    res += string.Format("<a href='{2}'><img src='/AdsFiles/Gif/{0}.gif' alt='{1}' title='{1}' style='margin:5px' width='{3}' /></a>", item.AdsId, item.Title, item.Link, Width);
                }
                else if (item.AdsType == 2)
                {
                    //------------Flash------
                    res += string.Format(@"<object width='{1}'  style='display: block' type='application/x-shockwave-flash'
                    data='/AdsFiles/Flash/{0}.swf'>
                    <param value='/AdsFiles/Flash/{0}.swf' name='movie'>
                    <param value='false' name='menu'>
                    </object>'", item.AdsId, Width);
                }
                else if (item.AdsType == 3)
                {
                    res += string.Format(item.Content);
                }
                else
                {
                    //------------Text------
                    res += string.Format("<a href='{0}' class='AdsTextTitle' >{1}</a><div class='AdsTextDesc'>{2}</div><br />", item.Link, item.Title, item.Content);
                }
                res += "</div>";
                allAds += res;
            }
            return allAds;
        }
    }
}