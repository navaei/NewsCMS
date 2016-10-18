using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Tazeyab.Common.Models.Mapping
{
    public class TagMap : EntityTypeConfiguration<Tag>
    {
        public TagMap()
        {
            this.ToTable("Tags");

            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Value)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.EnValue)
                .HasMaxLength(50);

            this.Property(t => t.Color)
                .HasMaxLength(10);

            this.Property(t => t.ImageThumbnail)
                .HasMaxLength(50);

            this.Property(t => t.BackgroundColor)
                .HasMaxLength(50);

            this.HasMany(t => t.Categories).WithMany(c => c.Tags);           
          
            this.HasMany(t => t.Users).WithMany(c => c.Tags).Map(m =>
            {
                m.ToTable("UsersTags").MapLeftKey("TagId").MapRightKey("UserId");
            });
        }
    }
}
