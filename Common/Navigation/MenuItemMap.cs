using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tazeyab.Common.Navigation
{
    public class MenuItemMap : EntityTypeConfiguration<MenuItem>
    {
        public MenuItemMap()
        {
            this.HasMany(m => m.Items).WithOptional(i => i.ParentItem).HasForeignKey(i => i.ParentItemId);
        }
    }
}
