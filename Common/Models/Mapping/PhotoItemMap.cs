using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Mn.NewsCms.Common.Models.Mapping
{
    public class PhotoItemMap : EntityTypeConfiguration<PhotoItem>
    {
        public PhotoItemMap()
        {
            this.ToTable("PhotoItems");

            // Properties
            this.Property(t => t.PhotoURL)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.PhotoThumbnail)
                .HasMaxLength(100);

            this.Property(t => t.Title)
                .HasMaxLength(100);

            // Relationships
            this.HasOptional(t => t.Category)
                .WithMany(t => t.PhotoItems)
                .HasForeignKey(d => d.CatId);

            this.HasRequired(p => p.Category).WithMany(c => c.PhotoItems);

        }
    }
}
