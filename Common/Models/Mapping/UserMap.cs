using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Tazeyab.Common.Models.Mapping
{
    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.UserName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Email)
                .HasMaxLength(150);

            this.Property(t => t.FirstName)
                .HasMaxLength(100);

            this.Property(t => t.LastName)
                .HasMaxLength(100);

            this.Property(t => t.Comment)
                .HasMaxLength(100);

            this.Property(t => t.Password)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.ConfirmationToken)
                .HasMaxLength(300);

            this.Property(t => t.PasswordVerificationToken)
                .HasMaxLength(300);

            // Table & Column Mappings
            this.ToTable("Users");
            //this.Property(t => t.UserId).HasColumnName("UserId");
            //this.Property(t => t.UserName).HasColumnName("UserName");
            //this.Property(t => t.LastActivityDate).HasColumnName("LastActivityDate");
            //this.Property(t => t.Email).HasColumnName("Email");
            //this.Property(t => t.FirstName).HasColumnName("FirstName");
            //this.Property(t => t.LastName).HasColumnName("LastName");
            //this.Property(t => t.Comment).HasColumnName("Comment");
            //this.Property(t => t.IsLockedOut).HasColumnName("IsLockedOut");
            //this.Property(t => t.IsApproved).HasColumnName("IsApproved");
            //this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            //this.Property(t => t.LastLoginDate).HasColumnName("LastLoginDate");
            //this.Property(t => t.LastPasswordChangedDate).HasColumnName("LastPasswordChangedDate");
            //this.Property(t => t.LastLockoutDate).HasColumnName("LastLockoutDate");
            //this.Property(t => t.Password).HasColumnName("Password");
            //this.Property(t => t.PasswordFailuresSinceLastSuccess).HasColumnName("PasswordFailuresSinceLastSuccess");
            //this.Property(t => t.LastPasswordFailureDate).HasColumnName("LastPasswordFailureDate");
            //this.Property(t => t.ConfirmationToken).HasColumnName("ConfirmationToken");
            //this.Property(t => t.PasswordVerificationToken).HasColumnName("PasswordVerificationToken");
            //this.Property(t => t.PasswordVerificationTokenExpirationDate).HasColumnName("PasswordVerificationTokenExpirationDate");
            //this.Property(t => t.Subscription).HasColumnName("Subscription");

            // Relationships
            this.HasMany(t => t.Tags)
                .WithMany(t => t.Users)
                .Map(m =>
                    {
                        m.ToTable("UsersTags");
                        m.MapLeftKey("UserId");
                        m.MapRightKey("TagId");
                    });

            this.HasMany(u => u.Roles).WithMany(r => r.Users);


        }
    }
}
