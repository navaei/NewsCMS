using System;
using System.Collections.Generic;

namespace Tazeyab.Common
{
    public class Application
    {
        public Application()
        {
            this.Memberships = new List<Membership>();
            this.Roles = new List<Role>();
        }

        public string ApplicationName { get; set; }
        public System.Guid ApplicationId { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Membership> Memberships { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
    }
}
