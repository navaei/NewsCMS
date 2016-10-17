using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Tazeyab.Common.Models.Mapping
{
    public class SocialTrackerMap : EntityTypeConfiguration<SocialTracker>
    {
        public SocialTrackerMap()
        {
            // Primary Key
            this.HasKey(t => t.SocialTrackerID);

            // Properties
            // Table & Column Mappings
            this.ToTable("SocialTracker");
            this.Property(t => t.SocialTrackerID).HasColumnName("SocialTrackerID");
            this.Property(t => t.FeedItemRef).HasColumnName("FeedItemRef");
            this.Property(t => t.ItemRef).HasColumnName("ItemRef");
            this.Property(t => t.SocialNetworkRef).HasColumnName("SocialNetworkRef");
            this.Property(t => t.CreationDate).HasColumnName("CreationDate");
        }
    }
}
