using System;
using System.Linq;
using Mn.NewsCms.Common.BaseClass;
using Mn.NewsCms.Common;

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

        public SearchHistoryBusiness(IUnitOfWork dbContext) : base(dbContext)
        {
        }
    }
}