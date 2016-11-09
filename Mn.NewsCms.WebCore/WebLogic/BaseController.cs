using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Mn.NewsCms.Common;
using Mn.NewsCms.Common.BaseClass;
using Controller = Microsoft.AspNetCore.Mvc.Controller;
using ViewContext = Microsoft.AspNetCore.Mvc.Rendering.ViewContext;

namespace Mn.NewsCms.WebCore.WebLogic
{
    public enum ControllerType
    {
        Tag, Cat, Key, Home, User
    }

    public abstract class BaseController : Controller
    {
        protected int PageSize { get { return 20; } }
        public virtual string EntityCode
        {
            get
            {
                //var controllerName = ControllerContext.ActionDescriptor.ControllerName;
                //controllerName = controllerName.Substring(controllerName.LastIndexOf(".") + 1,
                //    controllerName.Length - 11 - controllerName.LastIndexOf("."));
                return ControllerContext.ActionDescriptor.ControllerName;
            }
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewBag.DefAddress = Resources.Core.SiteUrl;
            ViewBag.PageSize = PageSize;
            ViewBag.EntityCode = EntityCode;
            ViewBag.Toggle = "1";

            ViewBag.Pages = new List<Post>();
            ViewBag.MainColumns = 9;

            base.OnActionExecuting(context);
        }

        //protected override void Initialize(RequestContext requestContext)
        //{
        //    base.Initialize(requestContext);
        //    //string path = Request.Url.ToString().Replace(Request.Url.AbsolutePath, "").Replace("http://", "");           
        //}

        protected string RenderViewToString(string viewName, object model)
        {
            viewName = viewName ?? ControllerContext.ActionDescriptor.ActionName;
            ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                IView view = ServiceFactory.Get<ICompositeViewEngine>().FindView(ControllerContext, viewName, true).View;
                ViewContext viewContext = new ViewContext(ControllerContext, view, ViewData, TempData, sw, new HtmlHelperOptions());

                view.RenderAsync(viewContext).Wait();

                return sw.GetStringBuilder().ToString();
            }
        }

        //public string RenderViewToString(string viewName, object model)
        //{
        //    if (string.IsNullOrEmpty(viewName))
        //        viewName = ControllerContext.RouteData.GetRequiredString("action");

        //    ViewData.Model = model;

        //    using (StringWriter sw = new StringWriter())
        //    {
        //        var engine = ServiceFactory.Get<ICompositeViewEngine>();
        //        ViewEngineResult viewResult = engine.FindView() ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
        //        ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
        //        viewResult.View.Render(viewContext, sw);
        //        return sw.GetStringBuilder().ToString();
        //    }
        //}      
    }
}