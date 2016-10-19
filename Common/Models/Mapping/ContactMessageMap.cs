using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Mn.NewsCms.Common.Models.Mapping
{
    public class ContactMessageMap : EntityTypeConfiguration<ContactMessage>
    {
        public ContactMessageMap()
        {
            this.ToTable("ContactMessages");


            // Properties
            this.Property(t => t.Title)
                .HasMaxLength(100);

            this.Property(t => t.Name)
                .HasMaxLength(100);

            this.Property(t => t.Email)
                .HasMaxLength(100);

            this.Property(t => t.Phone)
                .HasMaxLength(100);

            this.Property(t => t.Message)
                .HasMaxLength(1000);

           
        }
    }
}
