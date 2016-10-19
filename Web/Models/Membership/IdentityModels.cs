using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using Mn.NewsCms.Common.Membership;
using Mn.NewsCms.Common;

namespace Mn.NewsCms.Web.Models
{
    public class ApplicationUserStore :
    UserStore<User, Role, int,
    UserLogin, UserRole, UserClaim>, IUserStore<User, int>, IDisposable
    {
        public ApplicationUserStore()
            : this(new IdentityDbContext())
        {
            base.DisposeContext = true;
        }

        public ApplicationUserStore(DbContext context)
            : base(context)
        {
        }
    }


    public class ApplicationRoleStore
    : RoleStore<Role, int, UserRole>,
    IQueryableRoleStore<Role, int>,
    IRoleStore<Role, int>, IDisposable
    {
        public ApplicationRoleStore()
            : base(new IdentityDbContext())
        {
            base.DisposeContext = true;
        }

        public ApplicationRoleStore(DbContext context)
            : base(context)
        {
        }
    }
}