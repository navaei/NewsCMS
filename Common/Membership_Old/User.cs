using Mn.Framework.Common.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tazeyab.Common
{
    public class User : BaseEntity<Guid>
    {
        public User()
        {
            this.SearchHistories = new List<SearchHistory>();
            this.Roles = new List<Role>();
            this.Categories = new List<Category>();
            this.Sites = new List<Site>();
            this.Tags = new List<Tag>();
        }
        [Column("UserId")]
        public override Guid Id
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
        public string UserName { get; set; }
        public System.DateTime LastActivityDate { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Comment { get; set; }
        public bool IsLockedOut { get; set; }
        public bool IsApproved { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.DateTime LastLoginDate { get; set; }
        public System.DateTime LastPasswordChangedDate { get; set; }
        public System.DateTime LastLockoutDate { get; set; }
        public string Password { get; set; }
        public Nullable<int> PasswordFailuresSinceLastSuccess { get; set; }
        public Nullable<System.DateTime> LastPasswordFailureDate { get; set; }
        public string ConfirmationToken { get; set; }
        public string PasswordVerificationToken { get; set; }
        public Nullable<System.DateTime> PasswordVerificationTokenExpirationDate { get; set; }
        public Nullable<bool> Subscription { get; set; }
        public virtual Membership Membership { get; set; }
        public virtual Profile Profile { get; set; }
        public virtual ICollection<SearchHistory> SearchHistories { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Site> Sites { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
    }
}
