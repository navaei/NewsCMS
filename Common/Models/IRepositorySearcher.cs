using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mn.NewsCms.Common.Models
{
    public interface IRepositorySearcher
    {
        List<Guid> Search(string searchQuery, int PageSize, int PageIndex, LuceneBase.LuceneSearcherType SearchType, LuceneBase.LuceneSortField SortField, bool HasPhoto);

        FeedItem GetItem(string FeedItemId, long? ItemId);

        FeedItem GetItem(string FeedItemId, long? ItemId, bool DirectRequest);
    }
}
