using Mn.Framework.Business;
using Mn.Framework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mn.Framework.Common.Model;
using Mn.NewsCms.Common;
using Mn.NewsCms.Common.Models;

namespace Mn.NewsCms.DomainClasses.ContentManagment
{
    public class SiteBusiness : BaseBusiness<Site, long>, ISiteBusiness
    {

        public List<SiteOnlyTitle> GetTopSites(int TopCount, int Minutes = 20)
        {
            var maxTopSite = 22;
            var inCache = HttpContext.Current.Cache.Get("ViewBag_TopSites");
            if (inCache != null)
            {
                return ((List<SiteOnlyTitle>)inCache).Take(TopCount <= maxTopSite ? TopCount : maxTopSite).ToList();
            }
            var sites = SqlCommandSelect<SiteOnlyTitle>("Sites_SELECT_TOP {0}", maxTopSite).Result;
            HttpContext.Current.Cache.Insert("ViewBag_TopSites", sites, null, DateTime.Now.AddMinutes(Minutes), System.Web.Caching.Cache.NoSlidingExpiration);
            return sites.Take(TopCount <= maxTopSite ? TopCount : maxTopSite).ToList();
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

        public SiteBusiness(IUnitOfWork dbContext) : base(dbContext)
        {
        }
    }
}