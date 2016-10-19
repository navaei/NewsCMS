using Mn.Framework.Business;
using Mn.Framework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mn.NewsCms.Common;
using Mn.NewsCms.Common.Models;

namespace Mn.NewsCms.DomainClasses
{
    public class SearchHistoryBusiness : BaseBusiness<SearchHistory>, ISearchHistoryBusiness
    {
        IQueryable<SearchHistory> ISearchHistoryBusiness.GetList()
        {
            return base.GetList();
        }
        OperationStatus ISearchHistoryBusiness.Add(string SearchKey, long? Id, long? TagId, long? SiteId, Nullable<Guid> UserGuid)
        {
            return base.Create(new SearchHistory
            {
                CreationDate = DateTime.Now,
                SearchKey = SearchKey,
                Id = Id.Value,
                TagId = TagId,
                SiteId = SiteId,
                UserId = UserGuid
            });
        }

        OperationStatus ISearchHistoryBusiness.Add(Tag tag, Guid? UserId = new Nullable<Guid>())
        {
            return base.Create(new SearchHistory
            {
                CreationDate = DateTime.Now,
                SearchKey = string.IsNullOrEmpty(tag.EnValue) ? tag.Title.Trim() : tag.EnValue,
                TagId = tag.Id,
                UserId = UserId
            });
        }

        OperationStatus ISearchHistoryBusiness.Add(SearchHistory history)
        {
            return base.Create(history);
        }
    }
}