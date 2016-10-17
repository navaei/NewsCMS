using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Tazeyab.Common.Models.Mapping
{
    public class SiteMap : EntityTypeConfiguration<Site>
    {
        public SiteMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.SiteTitle)
                .IsRequired()
                .HasMaxLength(153);

            this.Property(t => t.SiteUrl)
                .IsRequired()
                .HasMaxLength(203);

            this.Property(t => t.SiteDesc)
                .HasMaxLength(200);

            //this.Property(t => t.SiteTags)
            //    .HasMaxLength(303);

            // Table & Column Mappings
            this.ToTable("Sites");
            this.Property(t => t.Id).HasColumnName("SiteId");
            //this.Property(t => t.SiteTitle).HasColumnName("SiteTitle");
            //this.Property(t => t.SiteUrl).HasColumnName("SiteUrl");
            //this.Property(t => t.SiteDesc).HasColumnName("SiteDesc");
            ////this.Property(t => t.SiteTags).HasColumnName("SiteTags");
            //this.Property(t => t.CrawledCount).HasColumnName("CrawledCount");
            //this.Property(t => t.LastCrawledDate).HasColumnName("LastCrawledDate");
            //this.Property(t => t.LinkedCount).HasColumnName("LinkedCount");
            //this.Property(t => t.ExternalLinkCount).HasColumnName("ExternalLinkCount");           
            //this.Property(t => t.HasFeed).HasColumnName("HasFeed");
            //this.Property(t => t.SkipType).HasColumnName("SkipType");
            //this.Property(t => t.IsBlog).HasColumnName("IsBlog");
            ////this.Property(t => t.SiteLogo).HasColumnName("SiteLogo");
            //this.Property(t => t.PageRank).HasColumnName("PageRank");
            //this.Property(t => t.HasSocialTag).HasColumnName("HasSocialTag");

            // Relationships
            this.HasMany(t => t.Users)
                .WithMany(t => t.Sites)
                .Map(m =>
                    {
                        m.ToTable("UsersSites");
                        m.MapLeftKey("SiteId");
                        m.MapRightKey("UserId");
                    });

            this.HasMany(s => s.Feeds).WithRequired(f => f.Site);

        }
    }
}
