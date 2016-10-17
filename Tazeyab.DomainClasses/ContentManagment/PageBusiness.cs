using Mn.Framework.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tazeyab.Common;

namespace Tazeyab.DomainClasses.ContentManagment
{
    public class PageBusiness : BaseBusiness<Page>, IPageBusiness
    {
        Page IPageBusiness.Get(int id)
        {
            return base.GetList().SingleOrDefault(p => p.Id == id);
        }
        Page IPageBusiness.Get(string code)
        {
            return base.GetList().SingleOrDefault(p => p.PageCode.ToLower() == code.ToLower());
        }

        IQueryable<Page> IPageBusiness.GetList()
        {
            return base.GetList();
        }

        public IQueryable<Page> GetByCats(List<int> catIds)
        {
            return (from p in base.GetList()
                    where
                    p.Active == true &&
                    p.Categories.Any(x => catIds.Contains(x.Id))
                    select p);
        }
        public IQueryable<Page> GetByTag(int tagId)
        {
            return (from p in base.GetList()
                    where
                    p.Active == true &&
                   p.Tags.Any(x => x.Id == tagId)
                    select p);

        }
    }
}
