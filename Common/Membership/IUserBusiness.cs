using Mn.Framework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tazeyab.Common.Membership;

namespace Tazeyab.Common
{
    public interface IUserBusiness
    {
        User Get(int userId);
        User Get(string userName);
        IQueryable<User> GetList();
        IQueryable<Role> GetRoleList();
        OperationStatus Update(User user);
        bool IsUserFlow(string EntityCode, string UserName, string Content);
        bool IsInRole(string userName, string role);

    }
}
