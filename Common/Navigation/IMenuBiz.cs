using Mn.Framework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tazeyab.Common.Navigation
{
    public interface IMenuBiz
    {
        Menu Get(MenuLocation location);
        Menu Get(string name);
        IQueryable<Menu> GetList();
        IQueryable<MenuItem> GetItems();
        IQueryable<MenuItem> GetItems(int menuId);
        OperationStatus CreateEdit(MenuItem item);
        OperationStatus DeleteItem(int itemId);
    }
}
