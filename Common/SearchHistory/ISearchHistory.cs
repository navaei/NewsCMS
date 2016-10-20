using System;
using System.Linq;
using OperationStatus = Mn.NewsCms.Common.BaseClass.OperationStatus;

namespace Mn.NewsCms.Common
{
    public interface ISearchHistoryBusiness
    {
        IQueryable<SearchHistory> GetList();
        OperationStatus Add(SearchHistory history);
        OperationStatus Add(string SearchKey, long? Id, long? TagId, long? SiteId, Nullable<Guid> UserGuid);
        OperationStatus Add(Tag tag, Guid? UserId = new Nullable<Guid>());
    }
}
