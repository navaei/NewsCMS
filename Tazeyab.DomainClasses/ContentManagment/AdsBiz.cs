using Mn.Framework.Business;
using Mn.Framework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tazeyab.Common;
using Tazeyab.Common.Models;

namespace Tazeyab.DomainClasses
{
    public class AdsBiz : BaseBusiness<Ad>, IAdsBiz
    {
        public int DefaultAdsWidth
        {
            get
            {
                return 120;
            }
        }
        public IQueryable<Ad> GetList()
        {
            return base.GetList();
        }
        public OperationStatus CreateEdit(Ad ads)
        {
            if (ads.Id == 0)
                return base.Create(ads);
            else
                return base.Update(ads);
        }
        public string GetAds(int width, int? catId, int? tagId)
        {
            if (width == 0)
                width = DefaultAdsWidth;
            string allAds = string.Empty;
            var ads = !catId.HasValue && !tagId.HasValue ? GetList().Where(x => x.Global == true && !x.Disable).ToList() : GetList(catId, tagId).ToList();
            int adscount = ads.Count();
            if (adscount < 10)
            {
                var ids = ads.Select(a => a.Id).ToList();
                var ads2 = GetList().Where(x => !ids.Contains(x.Id) && x.Global == true && !x.Disable).Take(10 - adscount).ToList();
                ads.AddRange(ads2);
            }
            ads = ads.OrderBy(x => x.AdsType).ToList();
            foreach (var item in ads)
            {
                string res = "<div class='DAdsItem'>";
                if (item.AdsType == AdsType.Gif)
                {
                    //---------Image--------
                    res += string.Format("<a href='{2}'><img src='/AdsFiles/Gif/{0}.gif' alt='{1}' title='{1}' style='margin:5px' width='{3}' /></a>", item.Id, item.Title, item.Link, width);
                }
                else if (item.AdsType == AdsType.Flash)
                {
                    //------------Flash------
                    res += string.Format(@"<object width='{1}'  style='display: block' type='application/x-shockwave-flash'
                    data='/AdsFiles/Flash/{0}.swf'>
                    <param value='/AdsFiles/Flash/{0}.swf' name='movie'>
                    <param value='false' name='menu'>
                    </object>'", item.Id, width);
                }
                else if (item.AdsType == AdsType.Script)
                {
                    res += string.Format(item.Content);
                }
                else if (item.AdsType == AdsType.Text)
                {
                    //------------Text------
                    res += string.Format("<a href='{0}' class='AdsTextTitle' >{1}</a><div class='AdsTextDesc'>{2}</div><br />", item.Link, item.Title, item.Content);
                }
                res += "</div>";
                allAds += res;
            }
            return allAds;
        }

        public IQueryable<Ad> GetList(int? catId, int? tagId)
        {
            if (catId.HasValue)
                return base.GetList().Where(a => a.Disable == false && a.Categories.Any(c => c.Id == catId.Value));
            else
                return base.GetList().Where(a => a.Disable == false && a.Tags.Any(t => t.Id == tagId.Value));

        }
    }
}
