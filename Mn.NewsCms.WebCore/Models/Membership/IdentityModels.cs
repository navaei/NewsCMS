using System;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Mn.NewsCms.Common.Membership;

namespace Mn.NewsCms.WebCore.Models.Membership
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