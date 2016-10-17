using Mn.Framework.Business;
using Mn.Framework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tazeyab.Common;
using Tazeyab.Common.Models;

namespace Tazeyab.DomainClasses
{
    public class RecentKeywordBusiness : BaseBusiness<RecentKeyWord>, IRecentKeywordBusiness
    {      
        List<RecentKeyWord> IRecentKeywordBusiness.GetList()
        {
            //--------------------Recent Tags----------------------
            if (!HttpContext.Current.Cache.Exist("RecentKeyWords"))
            {
                TazehaContext context = new TazehaContext();
                var Tags = context.RecentKeyWords.ToList();
                HttpContext.Current.Cache.AddToChache_Hours("RecentKeyWords", Tags, 1);
                context.Dispose();
                return Tags;
            }
            else
                return HttpContext.Current.Cache.Get("RecentKeyWords") as List<RecentKeyWord>;


        }
    }
}