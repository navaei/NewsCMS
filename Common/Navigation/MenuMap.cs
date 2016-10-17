using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tazeyab.Common.Navigation
{
    public class MenuMap : EntityTypeConfiguration<Menu>
    {
        public MenuMap()
        {
            this.HasMany(m => m.MenuItems).WithRequired(i => i.Menu).HasForeignKey(i => i.MenuId);
        }
    }
}
