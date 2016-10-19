using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Mn.Framework.Common.Model;
using System;

namespace Mn.NewsCms.Common.Membership
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class User : IdentityUser<int, UserLogin, UserRole, UserClaim>, IBaseEntity<int>
    {
        public User()
        {
            this.SearchHistories = new List<SearchHistory>();
            this.Categories = new List<Category>();
            this.Sites = new List<Site>();
            this.Tags = new List<Tag>();
            this.CreateDate = DateTime.Now;
        }
        public override int Id
        {
            get
            {
                return base.Id;
            }
            set
            {
                base.Id = value;
            }
        }
        [MaxLength(128)]
        [Display(Name = "نام")]
        public string FirstName { get; set; }
        [MaxLength(128)]
        [Display(Name = "نام خانوادگی")]
        public string LastName { get; set; }
        [Display(Name = "تاریخ ایجاد")]
        public DateTime CreateDate { get; set; }
        [Display(Name = "خبرنامه")]
        public Nullable<bool> Subscription { get; set; }
        public virtual ICollection<SearchHistory> SearchHistories { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Site> Sites { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User, int> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }


        public TViewModel ToViewModel<TViewModel>() where TViewModel : class, new()
        {
            return Mn.Framework.Helper.AutoMapper.Map<User, TViewModel>(this);
        }
    }
}