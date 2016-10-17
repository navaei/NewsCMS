using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Tazeyab.Common.Models.Mapping
{
    public class WebPartMap : EntityTypeConfiguration<WebPart>
    {
        public WebPartMap()
        {
            // Primary Key
            this.HasKey(t => t.WebPartId);

            // Properties
            this.Property(t => t.Code)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(10);

            this.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.HtmlContent)
                .IsRequired()
                .HasMaxLength(800);

            // Table & Column Mappings
            this.ToTable("WebParts");
            //this.Property(t => t.WebPartId).HasColumnName("WebPartId");
            //this.Property(t => t.Code).HasColumnName("Code");
            //this.Property(t => t.Title).HasColumnName("Title");
            //this.Property(t => t.HtmlContent).HasColumnName("HtmlContent");
            //this.Property(t => t.Global).HasColumnName("Global");

            this.HasMany(w => w.WebPartsContainers).WithRequired(c => c.WebPart);
        }
    }
}
