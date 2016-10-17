using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Tazeyab.Common.Models.Mapping
{
    public class Crawler_LogMap : EntityTypeConfiguration<Crawler_Log>
    {
        public Crawler_LogMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.PropertyNames)
                .IsRequired()
                .HasMaxLength(1024);

            this.Property(t => t.PropertyValues)
                .IsRequired()
                .HasMaxLength(1024);

            this.Property(t => t.Message)
                .HasMaxLength(1024);

            // Table & Column Mappings
            this.ToTable("Crawler_Log");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Id).HasColumnName("FeedId");
            this.Property(t => t.CreationDateTime).HasColumnName("CreationDateTime");
            this.Property(t => t.PropertyNames).HasColumnName("PropertyNames");
            this.Property(t => t.PropertyValues).HasColumnName("PropertyValues");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.Message).HasColumnName("Message");
            this.Property(t => t.NumberOfNewItem).HasColumnName("NumberOfNewItem");
        }
    }
}
