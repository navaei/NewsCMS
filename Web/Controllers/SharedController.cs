using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mn.NewsCms.Common;
using Mn.NewsCms.Common.Config;
using Mn.NewsCms.Common.Navigation;
using Mn.NewsCms.Web.Models;
using Mn.NewsCms.Web.WebLogic;

namespace Mn.NewsCms.Web.Controllers
{
    public partial class SharedController : BaseController
    {
        private readonly IAppConfigBiz _appConfigBiz;
        private readonly ICategoryBusiness _categoryBusiness;
        private readonly IMenuBiz _menuBiz;
        private readonly IPostBiz _postBiz;

        public SharedController(IAppConfigBiz appConfigBiz, ICategoryBusiness categoryBusiness, IMenuBiz menuBiz, IPostBiz postBiz)
        {
            _appConfigBiz = appConfigBiz;
            _categoryBusiness = categoryBusiness;
            _menuBiz = menuBiz;
            _postBiz = postBiz;
        }

        // GET: Share
        [OutputCache(Duration = CmsConfig.Cache3Hour)]
        public virtual ActionResult AnalysisScripts()
        {
            var gCode = _appConfigBiz.GetConfig<string>("GAnalyticsCode");
            return Content("<script type='text/javascript'>" + gCode + "</script>");
        }
        [OutputCache(Duration = CmsConfig.Cache3Hour)]
        public virtual ActionResult ShareInSocialScripts()
        {
            var addThisCode = _appConfigBiz.GetConfig<string>("AddThisCode");
            return Content(addThisCode);
        }
        [OutputCache(Duration = CmsConfig.Cache30Min)]
        public virtual ActionResult TopMenu()
        {
            var model = new MenuModel();
            model.Menu = _menuBiz.Get(MenuLocation.Top);
            if (model.Menu.EnableCategory && model.Menu.EnableCategory)
            {
                model.Categories = _categoryBusiness.CatsByViewMode_Cache(120, Mn.NewsCms.Common.Share.ViewMode.Menu, Mn.NewsCms.Common.Share.ViewMode.MenuIndex).ToList();
                model.Categories = model.Categories.OrderByDescending(x => x.Priority).ToList();
            }

            return PartialView("Navigation/_TopMenu." + CmsConfig.ThemeName, model);
        }
        public virtual ActionResult TopMenuMember()
        {
            return PartialView("Navigation/_TopMenuMember");
        }
        [OutputCache(Duration = CmsConfig.Cache3Hour)]
        public virtual ActionResult HeaderMenu()
        {
            var model = new MenuModel();
            model.Menu = _menuBiz.Get(MenuLocation.Top);
            if (model.Menu.EnableCategory && model.Menu.EnableCategory)
            {
                model.Categories = _categoryBusiness.CatsByViewMode_Cache(120, Mn.NewsCms.Common.Share.ViewMode.Menu, Mn.NewsCms.Common.Share.ViewMode.MenuIndex).ToList();
                model.Categories = model.Categories.OrderByDescending(x => x.Priority).ToList();
            }

            return PartialView("Navigation/_HeaderMenu." + CmsConfig.ThemeName, model);
        }
        [OutputCache(Duration = CmsConfig.Cache3Hour)]
        public virtual ActionResult Footer()
        {
            var model = _postBiz.Get(Constants.FooterPostName).Content;
            if (model.Contains("[footermenu]"))
            {
                var footerMenu = base.RenderViewToString(MVC.Shared.Views.Navigation._FooterMenu, new MenuModel() { Menu = _menuBiz.Get(Constants.Menus.FooterMenuName) });
                model = model.ReplaceAnyCase("[footermenu]", footerMenu);
            }
            if (model.Contains("[socialmenu]"))
            {
                var socialMenu = base.RenderViewToString(MVC.Shared.Views.Navigation._SocialMenu, new MenuModel() { Menu = _menuBiz.Get(Constants.Menus.SocialMenuName) });
                model = model.ReplaceAnyCase("[socialmenu]", socialMenu);
            }

            return PartialView(MVC.Shared.Views.Navigation._Footer, model);
        }

        [OutputCache(Duration = CmsConfig.Cache3Hour)]
        public virtual ActionResult FooterMenu()
        {
            var model = new MenuModel() { Menu = _menuBiz.Get(Constants.Menus.FooterMenuName) };
            return PartialView(MVC.Shared.Views.Navigation._FooterMenu, model);
        }
    }
}