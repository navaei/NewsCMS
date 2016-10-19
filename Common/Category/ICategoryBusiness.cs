using Mn.Framework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mn.NewsCms.Common.Models;
using Mn.NewsCms.Common.Share;

namespace Mn.NewsCms.Common
{
    public interface ICategoryBusiness
    {
        Category Get(long Id);
        Category Get(string content);
        IQueryable<Category> GetList();
        IQueryable<Category> GetList(long parentId);
        IQueryable<Category> GetList(List<long> ids);
        OperationStatus CreateEdit(Category cat);
        IEnumerable<Category> AllCats_Cache(int Minutes = 120);
        List<TagCatModel> AllCatsTags_Cache(int tagCount, int minutes = 120);
        IEnumerable<Category> CatsByViewMode_Cache(int Minutes, ViewMode viewMode);
        IEnumerable<Category> CatsByViewMode_Cache(int Minutes, ViewMode viewMode, ViewMode viewMode2);
        IEnumerable<Category> CatsByViewMode(ViewMode ViewMode);
        IEnumerable<Category> CatsByViewMode(ViewMode viewMode, ViewMode viewMode2);
    }
}
