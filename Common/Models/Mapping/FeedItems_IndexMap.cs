using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Mn.NewsCms.Common.Models.Mapping
{
    public class FeedItems_IndexMap : EntityTypeConfiguration<FeedItems_Index>
    {
        public FeedItems_IndexMap()
        {
            // Table & Column Mappings
            this.ToTable("FeedItems_Index");

            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Title)
                .HasMaxLength(300);

            this.Property(t => t.Description)
                .HasMaxLength(1000);

            this.Property(t => t.Link)
                .IsRequired()
                .HasMaxLength(400);

            this.Property(t => t.SiteURL)
                .HasMaxLength(300);

            this.Property(t => t.SiteTitle)
                .HasMaxLength(300);


        }
    }
}
