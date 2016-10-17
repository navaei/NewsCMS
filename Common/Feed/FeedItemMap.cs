using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Tazeyab.Common.Models.Mapping
{
    public class FeedItemMap : EntityTypeConfiguration<FeedItem>
    {
        public FeedItemMap()
        {
            this.ToTable("FeedItems");          
            // Relationships
            this.HasRequired(t => t.Feed).WithMany(t => t.FeedItems).HasForeignKey(d => d.FeedId);
        }
    }
}
