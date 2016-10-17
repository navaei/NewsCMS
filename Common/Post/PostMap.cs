using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tazeyab.Common
{
    public class PostMap : EntityTypeConfiguration<Post>
    {
        public PostMap()
        {
            this.HasMany(m => m.Categories).WithMany(c => c.Posts);
            this.HasMany(m => m.Tags).WithMany(c => c.Posts);
            this.HasMany(m => m.Comments).WithOptional(c => c.Post);
        }
    }
}
