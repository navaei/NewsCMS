using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Tazeyab.Common.Models.Mapping
{
    public class FeedMap : EntityTypeConfiguration<Feed>
    {
        public FeedMap()
        {
            this.ToTable("Feeds");

            //this.Property(t => t.Title)
            //    .IsRequired()
            //    .HasMaxLength(150);

            //this.Property(t => t.Description)
            //    .HasMaxLength(100);

            //this.Property(t => t.Link)
            //    .IsRequired()
            //    .HasMaxLength(400);

            //this.Property(t => t.CopyRight)
            //    .HasMaxLength(50);

            //this.Property(t => t.LastUpdatedItemUrl).HasMaxLength(400);

            //// Relationships       
            this.HasRequired(t => t.Site).WithMany(t => t.Feeds).HasForeignKey(d => d.SiteId);
            this.HasOptional(t => t.UpdateDuration).WithMany(t => t.Feeds).HasForeignKey(d => d.UpdateDurationId);
            this.HasMany(c => c.Tags).WithMany(t => t.Feeds);
            this.HasMany(c => c.Categories).WithMany(c => c.Feeds).Map(m =>
                {
                    m.ToTable("CatsFeeds")
                    .MapLeftKey("FeedId")
                    .MapRightKey("CatId");
                });

        }
    }
}
