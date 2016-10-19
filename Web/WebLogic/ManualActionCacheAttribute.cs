using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mn.NewsCms.Web.WebLogic
{
    public class ManualActionCacheAttribute : ActionFilterAttribute
    {
        public ManualActionCacheAttribute()
        {
        }

        public int Duration { get; set; }

        private string cachedKey = null;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string key = filterContext.HttpContext.Request.Url.PathAndQuery;
            this.cachedKey = "CustomResultCache-" + key.ToLower();
            if (filterContext.HttpContext.Cache[this.cachedKey] != null)
            {
                filterContext.Result = (ActionResult)filterContext.HttpContext.Cache[this.cachedKey];
            }
            else
            {
                base.OnActionExecuting(filterContext);
            }
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            filterContext.HttpContext.Cache.Add(this.cachedKey, filterContext.Result, null, DateTime.Now.AddSeconds(Duration), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Default, null);
            base.OnActionExecuted(filterContext);
        }
    }
}