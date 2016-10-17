using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Tazeyab.Common.Models.Mapping
{
    public class UpdateDurationMap : EntityTypeConfiguration<UpdateDuration>
    {
        public UpdateDurationMap()
        {
            // Table & Column Mappings
            this.ToTable("UpdateDuration");

            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Code)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(10);

            this.Property(t => t.DelayTime)
                .HasMaxLength(50);

        }
    }
}
