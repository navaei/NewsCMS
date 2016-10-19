using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Mn.NewsCms.Common.Models.Mapping
{
    public class ValueRefrenceMap : EntityTypeConfiguration<ValueRefrence>
    {
        public ValueRefrenceMap()
        {
            // Primary Key
            this.HasKey(t => new { t.TableName, t.ColumnName, t.ColumnValue, t.CoulmnDescription });

            // Properties
            this.Property(t => t.TableName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ColumnName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ColumnValue)
                .IsRequired()
                .HasMaxLength(150);

            this.Property(t => t.CoulmnDescription)
                .IsRequired()
                .HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("ValueRefrences");
            this.Property(t => t.TableName).HasColumnName("TableName");
            this.Property(t => t.ColumnName).HasColumnName("ColumnName");
            this.Property(t => t.ColumnValue).HasColumnName("ColumnValue");
            this.Property(t => t.CoulmnDescription).HasColumnName("CoulmnDescription");
        }
    }
}
