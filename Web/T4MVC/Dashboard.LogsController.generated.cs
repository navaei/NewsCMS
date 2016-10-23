// <auto-generated />
// This file was generated by a T4 template.
// Don't change it directly as your change would get overwritten.  Instead, make changes
// to the .tt file (i.e. the T4 template) and save it to regenerate this file.

// Make sure the compiler doesn't complain about missing Xml comments and CLS compliance
// 0108: suppress "Foo hides inherited member Foo. Use the new keyword if hiding was intended." when a controller and its abstract parent are both processed
// 0114: suppress "Foo.BarController.Baz()' hides inherited member 'Qux.BarController.Baz()'. To make the current member override that implementation, add the override keyword. Otherwise add the new keyword." when an action (with an argument) overrides an action in a parent controller
#pragma warning disable 1591, 3008, 3009, 0108, 0114
#region T4MVC

using System;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Routing;
using T4MVC;
namespace Mn.NewsCms.Web.Areas.Dashboard.Controllers
{
    public partial class LogsController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected LogsController(Dummy d) { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoute(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(Task<ActionResult> taskResult)
        {
            return RedirectToAction(taskResult.Result);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoutePermanent(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(Task<ActionResult> taskResult)
        {
            return RedirectToActionPermanent(taskResult.Result);
        }

        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.JsonResult Logs_Read()
        {
            return new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.Logs_Read);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult ClearCache()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ClearCache);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public LogsController Actions { get { return MVC.Dashboard.Logs; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "Dashboard";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "Logs";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "Logs";

        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string Index = "Index";
            public readonly string Logs_Read = "Logs_Read";
            public readonly string ClearCache = "ClearCache";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string Index = "Index";
            public const string Logs_Read = "Logs_Read";
            public const string ClearCache = "ClearCache";
        }


        static readonly ActionParamsClass_Index s_params_Index = new ActionParamsClass_Index();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Index IndexParams { get { return s_params_Index; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Index
        {
            public readonly string indb = "indb";
        }
        static readonly ActionParamsClass_Logs_Read s_params_Logs_Read = new ActionParamsClass_Logs_Read();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Logs_Read Logs_ReadParams { get { return s_params_Logs_Read; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Logs_Read
        {
            public readonly string request = "request";
            public readonly string indb = "indb";
        }
        static readonly ActionParamsClass_ClearCache s_params_ClearCache = new ActionParamsClass_ClearCache();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_ClearCache ClearCacheParams { get { return s_params_ClearCache; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_ClearCache
        {
            public readonly string indb = "indb";
        }
        static readonly ViewsClass s_views = new ViewsClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ViewsClass Views { get { return s_views; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ViewsClass
        {
            static readonly _ViewNamesClass s_ViewNames = new _ViewNamesClass();
            public _ViewNamesClass ViewNames { get { return s_ViewNames; } }
            public class _ViewNamesClass
            {
                public readonly string Index = "Index";
            }
            public readonly string Index = "~/Areas/Dashboard/Views/Logs/Index.cshtml";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public partial class T4MVC_LogsController : Mn.NewsCms.Web.Areas.Dashboard.Controllers.LogsController
    {
        public T4MVC_LogsController() : base(Dummy.Instance) { }

        [NonAction]
        partial void IndexOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, bool indb);

        [NonAction]
        public override System.Web.Mvc.ActionResult Index(bool indb)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Index);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "indb", indb);
            IndexOverride(callInfo, indb);
            return callInfo;
        }

        [NonAction]
        partial void Logs_ReadOverride(T4MVC_System_Web_Mvc_JsonResult callInfo, Kendo.Mvc.UI.DataSourceRequest request, bool indb);

        [NonAction]
        public override System.Web.Mvc.JsonResult Logs_Read(Kendo.Mvc.UI.DataSourceRequest request, bool indb)
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.Logs_Read);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "request", request);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "indb", indb);
            Logs_ReadOverride(callInfo, request, indb);
            return callInfo;
        }

        [NonAction]
        partial void ClearCacheOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, bool indb);

        [NonAction]
        public override System.Web.Mvc.ActionResult ClearCache(bool indb)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ClearCache);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "indb", indb);
            ClearCacheOverride(callInfo, indb);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591, 3008, 3009, 0108, 0114
