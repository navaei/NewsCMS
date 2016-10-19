using Mn.Framework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
