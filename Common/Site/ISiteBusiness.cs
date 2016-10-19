using Mn.Framework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mn.NewsCms.Common
{
    public interface ISiteBusiness
    {
        IQueryable<Site> GetList();
        Site Get(string siteUrl);
        IQueryable<Site> GetList(string siteUrl);
        List<SiteOnlyTitle> GetTopSites(int topCount, int CacheTimeMinutes = 20);
        OperationStatus Create(Site site);
        OperationStatus Update(Site site);
    }
}
