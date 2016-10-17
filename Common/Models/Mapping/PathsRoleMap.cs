using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Tazeyab.Common.Models.Mapping
{
    public class PathsRoleMap : EntityTypeConfiguration<PathsRole>
    {
        public PathsRoleMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("PathsRoles");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.PathId).HasColumnName("PathId");
            this.Property(t => t.RoleId).HasColumnName("RoleId");
        }
    }
}
