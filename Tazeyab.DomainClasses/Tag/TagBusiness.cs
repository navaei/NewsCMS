using Mn.Framework.Business;
using Mn.Framework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tazeyab.Common;
using Tazeyab.Common.Models;

namespace Tazeyab.DomainClasses.ContentManagment
{
    public class TagBusiness : BaseBusiness<Tag>, ITagBusiness
    {

        public Tag Get(long Id)
        {
            return base.GetList().SingleOrDefault(t => t.Id == Id);
        }
        public Tag Get(string content)
        {
            content = content.ToLower();
            return base.GetList().SingleOrDefault(c => c.Title.ToLower() == content || c.EnValue.ToLower() == content);
        }
        public OperationStatus Update(Tag tag)
        {
            return base.Update(tag);
        }
        public OperationStatus Delete(long id)
        {
            return base.Delete(id);
        }
        public List<Tag> TagsSelectTOP(bool InIndex, int TopCount)
        {
            var res = (List<Tag>)HttpContext.Current.Cache.Get("TagsTop");
            if (res == null)
            {
                res = base.GetList().Where(x => !InIndex || x.InIndex == InIndex).OrderByDescending(x => x.RepeatCount).Take(TopCount).ToList();
                HttpContext.Current.Cache.Insert("TagsTop", res, null, DateTime.Now.AddMinutes(1800), System.Web.Caching.Cache.NoSlidingExpiration);
                return res;
            }
            else
                return (List<Tag>)res;
        }

        public IQueryable<Tag> GetList()
        {
            return base.GetList();
        }
        public IQueryable<Tag> GetList(string term)
        {
            if (string.IsNullOrEmpty(term))
                return null;

            return base.GetList().Where(t => t.Title.StartsWith(term) || t.EnValue.StartsWith(term));
        }
        IEnumerable<RecentKeyWord> ITagBusiness.RelevantTags(string Content)
        {
            return base.GetList().Where(x => x.Value.Contains(Content) || Content.Contains(x.Value) || x.EnValue.Contains(Content) || Content.Contains(x.EnValue))
                .ToList()
                .Select(x => new RecentKeyWord
                {
                    IsTag = true,
                    Value = x.Title,
                    Title = x.Title
                });

        }

        public IQueryable<RecentKeyWord> GetListRecentKeyWord()
        {
            return base.GetList<RecentKeyWord>();
        }
        public List<RecentKeyWord> GetListRecentKeyWordCache()
        {
            //--------------------Recent Tags----------------------
            if (!HttpContext.Current.Cache.Exist("RecentKeyWords"))
            {
                var Tags = GetListRecentKeyWord().ToList();
                HttpContext.Current.Cache.AddToChache_Hours("RecentKeyWords", Tags, 1);
                return Tags;
            }
            else
                return HttpContext.Current.Cache.Get("RecentKeyWords") as List<RecentKeyWord>;


        }

        public OperationStatus UpdateHotKey(RecentKeyWord key)
        {
            return base.Update<RecentKeyWord>(key);
        }

        public OperationStatus CreateHotKey(RecentKeyWord key)
        {
            return base.Create<RecentKeyWord>(key);
        }
        public OperationStatus DeleteHotKey(RecentKeyWord key)
        {
            return base.Delete<RecentKeyWord>(key.Id);
        }
    }
}