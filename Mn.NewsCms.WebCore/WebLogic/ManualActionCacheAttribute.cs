using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Mn.NewsCms.Common.BaseClass;

namespace Mn.NewsCms.WebCore.WebLogic
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
            string key = filterContext.HttpContext.Request.Path;
            this.cachedKey = "CustomResultCache-" + key.ToLower();
            if (ServiceFactory.Get<ICacheManager>().IsSet(this.cachedKey))
            {
                filterContext.Result = ServiceFactory.Get<ICacheManager>().Get<ActionResult>(this.cachedKey);
            }
            else
            {
                base.OnActionExecuting(filterContext);
            }
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {

            ServiceFactory.Get<ICacheManager>().Set(this.cachedKey, filterContext.Result, Duration / 60);
            base.OnActionExecuted(filterContext);
        }
    }
}