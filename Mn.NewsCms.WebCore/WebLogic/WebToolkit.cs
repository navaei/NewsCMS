using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http.Extensions;

namespace Mn.NewsCms.WebCore.WebLogic
{
    public class WebToolkit
    {
        public class MandatoryWww : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                if (!(filterContext.HttpContext.Request.Host.Port.HasValue && filterContext.HttpContext.Request.Host.Port.Value != 80))
                {
                    string url = filterContext.HttpContext.Request.GetEncodedUrl().ToLowerInvariant();
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