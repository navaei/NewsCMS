using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tazeyab.Common
{
    public interface IPageBusiness
    {
        Page Get(int id);
        Page Get(string code);
        IQueryable<Page> GetList();
        IQueryable<Page> GetByCats(List<int> catIds);
        IQueryable<Page> GetByTag(int tagId);
    }
}
