﻿using System.Web.Mvc;

namespace Mn.NewsCms.Web.Areas.Dashboard
{
    public class dashboardAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Dashboard";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "dashboard_default",
                "dashboard/{controller}/{action}/{id}",
                new { controller = "home", action = "Index", id = UrlParameter.Optional },
                new[] { "Mn.NewsCms.Web.Areas.Dashboard.Controllers" }
            );
        }
    }
}