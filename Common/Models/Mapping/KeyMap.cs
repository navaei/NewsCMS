using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Tazeyab.Common.Models.Mapping
{
    public class KeyMap : EntityTypeConfiguration<Key>
    {
        public KeyMap()
        {
            // Primary Key
            this.HasKey(t => t.KeyId);

            // Properties
            this.Property(t => t.Value)
                .IsRequired()
                .HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("Keys");
            this.Property(t => t.KeyId).HasColumnName("KeyId");
            this.Property(t => t.Value).HasColumnName("Value");
            this.Property(t => t.FeedItemsId).HasColumnName("FeedItemsId");
            this.Property(t => t.ConversionNumber).HasColumnName("ConversionNumber");
         

        }
    }
}
