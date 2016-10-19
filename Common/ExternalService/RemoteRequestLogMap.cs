using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Mn.NewsCms.Common.Models.Mapping
{
    public class RemoteRequestLogMap : EntityTypeConfiguration<RemoteRequestLog>
    {
        public RemoteRequestLogMap()
        {
            //// Primary Key
            //this.HasKey(t => t.ID);

            //// Properties
            //this.Property(t => t.RequestRefer)
            //    .IsRequired()
            //    .HasMaxLength(50);

            //this.Property(t => t.Controller)
            //    .IsRequired()
            //    .IsFixedLength()
            //    .HasMaxLength(20);

            //this.Property(t => t.Content)
            //    .IsRequired()
            //    .HasMaxLength(50);

            //// Table & Column Mappings
            //this.ToTable("RemoteRequestLogs");
            //this.Property(t => t.ID).HasColumnName("ID");
            //this.Property(t => t.RequestRefer).HasColumnName("RequestRefer");
            //this.Property(t => t.Controller).HasColumnName("Controller");
            //this.Property(t => t.Content).HasColumnName("Content");
            //this.Property(t => t.CreationDate).HasColumnName("CreationDate");
        }
    }
}
