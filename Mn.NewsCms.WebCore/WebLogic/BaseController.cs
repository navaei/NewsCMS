using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using Mn.NewsCms.Common;
using Controller = Microsoft.AspNetCore.Mvc.Controller;
using ViewContext = Microsoft.AspNetCore.Mvc.Rendering.ViewContext;
using ViewEngineResult = Microsoft.AspNetCore.Mvc.ViewEngines.ViewEngineResult;

namespace Mn.NewsCms.WebCore.WebLogic
{
    public enum ControllerType
    {
        Tag, Cat, Key, Home, User
    }

    public abstract class BaseController : Controller
    {
        protected int PageSize { get { return 20; } }
        //public virtual string EntityCode
        //{
        //    get
        //    {
        //        var controllerName = ControllerContext.Controller.ToString();
        //        controllerName = controllerName.Substring(controllerName.LastIndexOf(".") + 1,
        //            controllerName.Length - 11 - controllerName.LastIndexOf("."));
        //        return controllerName;
        //    }
        //}
        //protected override void Initialize(RequestContext requestContext)
        //{
        //    base.Initialize(requestContext);
        //    //string path = Request.Url.ToString().Replace(Request.Url.AbsolutePath, "").Replace("http://", "");
        //    ViewBag.DefAddress = Resources.Core.SiteUrl;
        //    ViewBag.PageSize = PageSize;
        //    ViewBag.EntityCode = EntityCode;
        //    ViewBag.Toggle = "1";

        //    ViewBag.Pages = new List<Post>();
        //    ViewBag.MainColumns = 9;
        //}

        public string RenderViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                return sw.GetStringBuilder().ToString();
            }
        }
    }
}