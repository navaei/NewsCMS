using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tazeyab.Web.Common
{
    public enum ControllerType
    {
        Tag, Cat, Key, Home, User
    }
    public class XController : System.Web.Mvc.Controller
    {

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            //string path = Request.Url.ToString().Replace(Request.Url.AbsolutePath, "").Replace("http://", "");
            ViewBag.DefAddress = "www.Tazeyab.com";
            //if (Request.Url.AbsolutePath.IndexOfX("/Cat") > -1)
            //    ViewBag.ControllerType = ControllerType.Cat;
            //if (Request.Url.AbsolutePath.IndexOfX("/Tag") > -1)
            //    ViewBag.ControllerType = ControllerType.Tag;
            //if (Request.Url.AbsolutePath.IndexOfX("/Key") > -1)
            //    ViewBag.ControllerType = ControllerType.Key;
            //if (Request.Url.AbsolutePath.IndexOfX("/User") > -1)
            //    ViewBag.ControllerType = ControllerType.User;
        }
    }
}