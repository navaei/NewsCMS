using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Mn.NewsCms.Common.Models.Mapping
{
    public class LogsBufferMap : EntityTypeConfiguration<LogsBuffer>
    {
        public LogsBufferMap()
        {
            // Primary Key
            //this.HasKey(t => new { t.Value, t.CreationDate });

            // Properties
            //this.Property(t => t.Value)
            //    .IsRequired()
            //    .HasMaxLength(1024);

            // Table & Column Mappings
            //this.ToTable("LogsBuffer");
            //this.Property(t => t.Value).HasColumnName("Value");
            //this.Property(t => t.Code).HasColumnName("Code");
            //this.Property(t => t.Type).HasColumnName("TypeNumber");
            //this.Property(t => t.CreationDate).HasColumnName("CreationDate");
        }
    }
}
