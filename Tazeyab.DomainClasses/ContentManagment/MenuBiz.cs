using System.Linq;
using Mn.NewsCms.Common.BaseClass;
using Mn.NewsCms.Common.Navigation;

namespace Mn.NewsCms.DomainClasses
{
    public class MenuBiz : BaseBusiness<Menu>, IMenuBiz
    {
        private readonly IUnitOfWork _dbContext;

        public MenuBiz(IUnitOfWork dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public Menu Get(MenuLocation location)
        {
            return base.GetList().SingleOrDefault(m => m.Location == location);
        }
        public Menu Get(string name)
        {
            return base.GetList().SingleOrDefault(m => m.Name.ToLower() == name.ToLower());
        }
        public IQueryable<Menu> GetList()
        {
            return base.GetList();
        }
        public OperationStatus CreateEdit(MenuItem item)
        {
            if (item.Id == 0)
            {
                return base.Create<MenuItem>(item);
            }
            else
                return base.Update<MenuItem>(item);

        }
        public OperationStatus DeleteItem(int itemId)
        {
            var item = base.GetList<MenuItem>().SingleOrDefault(i => i.Id == itemId);
            _dbContext.Set<MenuItem>().Remove(item);
            var res = _dbContext.SaveAllChanges() > 0;
            return new OperationStatus() { Status = res };
        }

        public IQueryable<MenuItem> GetItems()
        {
            return base.GetList<MenuItem>();
        }

        public IQueryable<MenuItem> GetItems(int menuId)
        {
            return base.GetList<MenuItem>().Where(m => m.MenuId == menuId);
        }
    }
}
