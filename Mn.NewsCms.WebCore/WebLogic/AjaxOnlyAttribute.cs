using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Mn.NewsCms.WebCore.WebLogic
{
    public class AjaxOnlyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {            
            if (!filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new BadRequestResult();
            }
        }
    }
}