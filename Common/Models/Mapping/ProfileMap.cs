using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Tazeyab.Common.Models.Mapping
{
    public class ProfileMap : EntityTypeConfiguration<Profile>
    {
        public ProfileMap()
        {
            // Primary Key
            this.HasKey(t => t.UserId);

            // Properties
            this.Property(t => t.PropertyNames)
                .IsRequired()
                .HasMaxLength(4000);

            this.Property(t => t.PropertyValueStrings)
                .IsRequired()
                .HasMaxLength(4000);

            this.Property(t => t.PropertyValueBinary)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("Profiles");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.PropertyNames).HasColumnName("PropertyNames");
            this.Property(t => t.PropertyValueStrings).HasColumnName("PropertyValueStrings");
            this.Property(t => t.PropertyValueBinary).HasColumnName("PropertyValueBinary");
            this.Property(t => t.LastUpdatedDate).HasColumnName("LastUpdatedDate");

            // Relationships
            this.HasRequired(t => t.User)
                .WithOptional(t => t.Profile);

        }
    }
}
