using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mn.NewsCms.Web.WebLogic
{
    public class WebToolkit
    {
        public class MandatoryWww : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                if (!filterContext.RequestContext.HttpContext.Request.IsLocal)
                {
                    string url = filterContext.RequestContext.HttpContext.Request.Url.AbsoluteUri.ToLowerInvariant();
                    if (!url.Contains("www"))
                    {
                        url = url.Replace("http://", "http://www.");
                        //url = url.Replace("https://", "https://www.");
                        filterContext.Result = new RedirectResult(url, true);
                    }
                }
                base.OnActionExecuting(filterContext);
            }
        }
    }
}