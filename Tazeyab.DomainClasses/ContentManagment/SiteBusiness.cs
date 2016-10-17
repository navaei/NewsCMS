using Mn.Framework.Business;
using Mn.Framework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tazeyab.Common;
using Tazeyab.Common.Models;

namespace Tazeyab.DomainClasses.ContentManagment
{
    public class SiteBusiness : BaseBusiness<Site, long>, ISiteBusiness
    {

        public List<SiteOnlyTitle> GetTopSites(int TopCount, int Minutes = 20)
        {
            int MaxTopSite = 22;
            var inCache = HttpContext.Current.Cache.Get("ViewBag_TopSites");
            if (inCache != null)
            {
                return ((List<SiteOnlyTitle>)inCache).Take(TopCount <= MaxTopSite ? TopCount : MaxTopSite).ToList();
            }
            //List<Site> sites = new List<Site>();
            List<SiteOnlyTitle> sites = base.DataContext.Database.SqlQuery<SiteOnlyTitle>("Sites_SELECT_TOP {0}", MaxTopSite).ToList();
            //var dic = context.Database.SqlQuery<SiteOnlyTitle>("Sites_SELECT_TOP {0}", TopCount);
            HttpContext.Current.Cache.Insert("ViewBag_TopSites", sites, null, DateTime.Now.AddMinutes(Minutes), System.Web.Caching.Cache.NoSlidingExpiration);
            return sites.Take(TopCount <= MaxTopSite ? TopCount : MaxTopSite).ToList();
        }

        IQueryable<Site> ISiteBusiness.GetList()
        {
            return base.GetList();
        }

        Site ISiteBusiness.Get(string siteUrl)
        {
            siteUrl = siteUrl.Trim().ToLower();
            return base.GetList().SingleOrDefault(s => s.SiteUrl.ToLower() == siteUrl);
        }
        IQueryable<Site> ISiteBusiness.GetList(string siteUrl)
        {
            siteUrl = siteUrl.Trim().ToLower().ReplaceAnyCase("www.", "").ReplaceAnyCase("http://", "").Replace("/", "");
            return base.GetList().Where(s => s.SiteUrl.ToLower().StartsWith(siteUrl) || siteUrl.StartsWith(s.SiteUrl));
        }
        OperationStatus ISiteBusiness.Create(Site site)
        {
            return base.Create(site);
        }
        OperationStatus ISiteBusiness.Update(Site site)
        {
            return base.Update(site);
        }
    }
}