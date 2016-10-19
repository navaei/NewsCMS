using Mn.Framework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mn.NewsCms.Common.Models;

namespace Mn.NewsCms.Common
{
    public interface ITagBusiness
    {
        Tag Get(long Id);
        Tag Get(string content);
        IQueryable<Tag> GetList();
        IQueryable<Tag> GetList(string term);
        OperationStatus Update(Tag tag);
        OperationStatus Delete(long id);
        List<Tag> TagsSelectTOP(bool InIndex, int TopCount);
        IEnumerable<RecentKeyWord> RelevantTags(string Content);
        IQueryable<RecentKeyWord> GetListRecentKeyWord();
        List<RecentKeyWord> GetListRecentKeyWordCache();
        OperationStatus UpdateHotKey(RecentKeyWord key);
        OperationStatus CreateHotKey(RecentKeyWord key);
        OperationStatus DeleteHotKey(RecentKeyWord key);

    }
}
