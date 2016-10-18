using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Tazeyab.Common.Models.Mapping
{
    public class CategoryMap : EntityTypeConfiguration<Category>
    {
        public CategoryMap()
        {
            this.ToTable("Categories");

            // Properties
            this.Property(t => t.Title)
                .HasMaxLength(200);

            this.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Description)
                .HasMaxLength(1000);          

            this.Property(t => t.KeyWords)
                .HasMaxLength(500);

            this.Property(t => t.Color)
                .HasMaxLength(10);

            this.Property(t => t.Icon)
                .HasMaxLength(3000);

            this.Property(t => t.ImageThumbnail)
                .HasMaxLength(50); 

            // Relationships
            this.HasMany(c => c.Categories).WithOptional(c1 => c1.ParentCategory).Map(c1 => c1.MapKey("ParentId"));
            this.HasMany(t => t.Users)
                .WithMany(t => t.Categories)
                .Map(m =>
                    {
                        m.ToTable("UsersCats");
                        m.MapLeftKey("CatId");
                        m.MapRightKey("UserId");
                    });
            this.HasMany(c => c.Tags).WithMany(t => t.Categories).Map(m =>
            {
                m.ToTable("TagCategorie").MapLeftKey("Categorie_CatId").MapRightKey("Tag_TagId");
            });                     
        }
    }
}
