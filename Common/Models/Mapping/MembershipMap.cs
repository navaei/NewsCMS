using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Tazeyab.Common.Models.Mapping
{
    public class MembershipMap : EntityTypeConfiguration<Membership>
    {
        public MembershipMap()
        {
            this.ToTable("Memberships");

            // Properties
            this.Property(t => t.Password)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.PasswordSalt)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.Email)
                .HasMaxLength(256);

            this.Property(t => t.PasswordQuestion)
                .HasMaxLength(256);

            this.Property(t => t.PasswordAnswer)
                .HasMaxLength(128);

            this.Property(t => t.Comment)
                .HasMaxLength(256);

            // Relationships
            this.HasRequired(t => t.Application)
                .WithMany(t => t.Memberships)
                .HasForeignKey(d => d.ApplicationId);
            this.HasRequired(t => t.User)
                .WithOptional(t => t.Membership);

        }
    }
}
