﻿using Kendo.Mvc.UI;
using System.Linq;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Mn.NewsCms.Common;
using Mn.NewsCms.Common.BaseClass;
using Mn.NewsCms.Robot.Helper;
using Mn.NewsCms.Web.Models;
using Mn.NewsCms.Web.WebLogic.BaseModel;

namespace Mn.NewsCms.Web.Areas.Dashboard.Controllers
{
    public partial class SiteController : BaseAdminController
    {
        private readonly ISiteBusiness _siteBusiness;

        public SiteController(ISiteBusiness siteBusiness)
        {
            _siteBusiness = siteBusiness;
        }

        public virtual ActionResult Index()
        {
            var model = new PageGridModel();
            model.GridMenu = new ColumnActionMenu(new ColumnActionMenu.ActionMenuItem(ColumnActionMenu.ItemType.ScriptCommand, Mn.NewsCms.Common.Resources.General.Edit, "openModal('/Dashboard/Site/Manage/#=Id#')"),
                new ColumnActionMenu.ActionMenuItem(ColumnActionMenu.ItemType.ScriptCommand, Mn.NewsCms.Common.Resources.General.Delete, "deleteGridRow('/Dashboard/Site/Delete/#=Id#')"),
                new ColumnActionMenu.ActionMenuItem(ColumnActionMenu.ItemType.ActionLink, "مشاهده فیدها", "/Dashboard/Feed/?siteId=#=Id#"));
            return View(model);
        }
        public virtual JsonResult Site_Read([DataSourceRequest]DataSourceRequest request, string url, string title)
        {
            var query = _siteBusiness.GetList();

            if (!request.Sorts.Any())
                request.Sorts.Add(new Kendo.Mvc.SortDescriptor("Id", System.ComponentModel.ListSortDirection.Descending));

            var sites = query.Select(s => new SiteViewModel
            {
                Id = s.Id,
                SiteTitle = s.SiteTitle,
                SiteUrl = s.SiteUrl,
                PageRank = s.PageRank,
                CrawledCount = s.CrawledCount,
                HasFeed = s.HasFeed,
                IsBlog = s.IsBlog,
                HasSocialTag = s.HasSocialTag,
                SkipType = s.SkipType,
                ShowContentType = s.ShowContentType
            });

            return Json(sites.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        public virtual JsonResult Site_Read_Summary()
        {
            var param = Request.Params["text"].ToString();
            var query = _siteBusiness.GetList().Where(s => s.SiteUrl.Contains(param) || s.SiteTitle.Contains(param)).Take(10).Select(s => new TitleValue<long>
            {
                Value = s.Id,
                Title = s.SiteTitle,
            });
            return Json(query, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public virtual ActionResult Manage(long? id, string url)
        {
            var model = new SiteViewModel();

            if (!string.IsNullOrEmpty(url))
            {
                var sites = _siteBusiness.GetList(url);
                if (sites.Any())
                {
                    model = sites.First().ToViewModel<SiteViewModel>();
                }
                else
                {
                    if (!url.ToLower().StartsWith("http"))
                        url = "http://" + url;
                    var page = Requester.GetPage(url);
                    model.SiteUrl = url.ReplaceAnyCase("www.", "").ReplaceAnyCase("http://", "").ReplaceAnyCase("https://", "").Replace("/", "");
                    model.SiteTitle = page.Title;
                    model.SiteDesc = page.Description;
                    model.IsBlog = model.SiteUrl.IndexOf('.') != model.SiteUrl.LastIndexOf('.') ? true : false;
                }

            }

            if (id.HasValue)
            {
                model = _siteBusiness.GetList().SingleOrDefault(s => s.Id == id).ToViewModel<SiteViewModel>();
            }

            return View(model);
        }

        [HttpPost]
        public virtual JsonResult Manage(SiteViewModel site)
        {
            OperationStatus status;
            site.SiteUrl = site.SiteUrl.ReplaceAnyCase("www.", "").Replace("http://", "").Replace("/", "");

            if (site.Id == 0)
                status = _siteBusiness.Create(site.ToModel<Site>());
            else
            {
                var dbSite = _siteBusiness.GetList().SingleOrDefault(s => s.Id == site.Id);

                dbSite.SiteTitle = site.SiteTitle;
                dbSite.SiteUrl = site.SiteUrl;
                dbSite.SiteDesc = site.SiteDesc;

                status = _siteBusiness.Update(site.ToModel<Site>());
            }

            return Json(status.ToJOperationResult(), JsonRequestBehavior.AllowGet);
        }
    }
}