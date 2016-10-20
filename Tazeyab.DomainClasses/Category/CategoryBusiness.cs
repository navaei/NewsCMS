using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mn.NewsCms.Common.BaseClass;
using Mn.NewsCms.Common;
using Mn.NewsCms.Common.Models;
using Mn.NewsCms.Common.Share;
using Mn.NewsCms.DomainClasses.ContentManagment;

namespace Mn.NewsCms.DomainClasses
{
    public class CategoryBusiness : BaseBusiness<Category>, ICategoryBusiness
    {
        private readonly ITagBusiness _tagBusiness;
        public CategoryBusiness(IUnitOfWork dbContext, ITagBusiness tagBusiness) : base(dbContext)
        {
            _tagBusiness = tagBusiness;
        }
        Category ICategoryBusiness.Get(long Id)
        {
            return GetList().SingleOrDefault(c => c.Id == Id);
        }
        Category ICategoryBusiness.Get(string content)
        {
            content = content.Trim().ToLower();
            return GetList().SingleOrDefault(c => c.Title == content || c.Code == content);
        }
        IQueryable<Category> ICategoryBusiness.GetList()
        {
            return base.GetList();
        }
        IQueryable<Category> ICategoryBusiness.GetList(long parentId)
        {
            return GetList().Where(c => c.ParentId.HasValue && c.ParentId.Value == parentId);
        }
        IQueryable<Category> ICategoryBusiness.GetList(List<long> ids)
        {
            return GetList().Where(c => ids.Contains(c.Id));
        }

        OperationStatus ICategoryBusiness.CreateEdit(Category cat)
        {
            OperationStatus res;
            if (cat.Id == 0)
                res = base.Create(cat);
            else
                res = base.Update(cat);

            return res;
        }
        IEnumerable<Category> ICategoryBusiness.AllCats_Cache(int Minutes = 120)
        {
            if (HttpContext.Current.Cache.Get("AllCats_Cache") == null)
                HttpContext.Current.Cache.Insert("AllCats_Cache", GetList().ToList(), null, DateTime.Now.AddMinutes(Minutes), System.Web.Caching.Cache.NoSlidingExpiration);

            return ((IEnumerable<Category>)HttpContext.Current.Cache.Get("AllCats_Cache"));
        }
        List<TagCatModel> ICategoryBusiness.AllCatsTags_Cache(int tagCount, int minutes = 120)
        {
            var cats = GetList().Select(c => new TagCatModel() { Link = "cat/" + c.Code, Title = c.Title, Color = c.Color }).ToList();
            var tags = _tagBusiness.GetList().Shuffle().Take(tagCount).Select(c => new TagCatModel() { Link = "tag/" + c.Value, Title = c.Title, Color = c.Color }).ToList();
            cats.AddRange(tags);
            if (HttpContext.Current.Cache.Get("AllCatsTags_Cache") == null)
                HttpContext.Current.Cache.Insert("AllCatsTags_Cache", cats, null, DateTime.Now.AddMinutes(minutes), System.Web.Caching.Cache.NoSlidingExpiration);

            return (List<TagCatModel>)HttpContext.Current.Cache.Get("AllCatsTags_Cache");
        }
        IEnumerable<Category> ICategoryBusiness.CatsByViewMode_Cache(int Minutes, ViewMode ViewMode, ViewMode viewMode2)
        {
            if (HttpContext.Current.Cache.Get("CatsByViewMode" + ViewMode) == null)
                HttpContext.Current.Cache.Insert("CatsByViewMode" + ViewMode, CatsByViewMode(ViewMode, viewMode2), null, DateTime.Now.AddMinutes(Minutes), System.Web.Caching.Cache.NoSlidingExpiration);

            return ((IEnumerable<Category>)HttpContext.Current.Cache.Get("CatsByViewMode" + ViewMode));
        }
        IEnumerable<Category> ICategoryBusiness.CatsByViewMode_Cache(int Minutes, ViewMode ViewMode)
        {
            if (HttpContext.Current.Cache.Get("CatsByViewMode" + ViewMode) == null)
                HttpContext.Current.Cache.Insert("CatsByViewMode" + ViewMode, CatsByViewMode(ViewMode), null, DateTime.Now.AddMinutes(Minutes), System.Web.Caching.Cache.NoSlidingExpiration);

            return ((IEnumerable<Category>)HttpContext.Current.Cache.Get("CatsByViewMode" + ViewMode));
        }
        public IEnumerable<Category> CatsByViewMode(ViewMode ViewMode)
        {
            return GetList().Where(x => x.ViewMode == ViewMode).ToList();
        }
        public IEnumerable<Category> CatsByViewMode(ViewMode ViewMode, ViewMode viewMode2)
        {
            return GetList().Where(x => x.ViewMode == ViewMode || x.ViewMode == viewMode2).ToList();
        }


    }
}