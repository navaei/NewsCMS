using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tazeyab.Common.Models;

namespace Tazeyab.Common.Share
{
    public interface IRecomendBiz
    {
        List<SiteOnlyTitle> Sites(int TopCount);
        List<CategoryModel> Cats(int TopCount);
        List<Tag> Tags(int TopCount);
        List<SiteOnlyTitle> Sites(int TopCount, List<long> userSites);
        List<CategoryModel> Cats(int TopCount, List<long> userCats);
        List<Tag> Tags(int TopCount, List<long> userTags);
        List<FeedItems_Index> FeedItemsIndex(int TopCount);
        List<FeedItem> FeedItems(int TopCount);
    }
}
