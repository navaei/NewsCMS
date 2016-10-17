using System;
using System.Linq;
using System.Web.Security;
using Tazeyab.Common;
using Tazeyab.Common.Models;
using Tazeyab.DomainClasses.Security;

namespace Tazeyab.Web.WebLogic
{
    public class CodeFirstMembershipProvider : MembershipProvider
    {
        //public CodeFirstMembershipProvider()
        //{ 
        
        //}
        //public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        //{
        //    //base.Initialize(name, config);
        //}

        #region Properties

        public override string ApplicationName
        {
            get
            {
                return this.GetType().Assembly.GetName().Name.ToString();
            }
            set
            {
                this.ApplicationName = this.GetType().Assembly.GetName().Name.ToString();
            }
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { return 5; }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return 0; }
        }

        public override int MinRequiredPasswordLength
        {
            get { return 6; }
        }

        public override int PasswordAttemptWindow
        {
            get { return 0; }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { return MembershipPasswordFormat.Hashed; }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { return String.Empty; }
        }

        public override bool RequiresUniqueEmail
        {
            get { return true; }
        }

        #endregion

        #region Functions

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            if (string.IsNullOrEmpty(username))
            {
                status = MembershipCreateStatus.InvalidUserName;
                return null;
            }
            if (string.IsNullOrEmpty(password))
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }
            if (string.IsNullOrEmpty(email))
            {
                status = MembershipCreateStatus.InvalidEmail;
                return null;
            }

            string HashedPassword = Crypto.HashPassword(password);
            if (HashedPassword.Length > 128)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }

            using (TazehaContext Context = new TazehaContext())
            {
                if (Context.Users.Where(Usr => Usr.UserName == username).Any())
                {
                    status = MembershipCreateStatus.DuplicateUserName;
                    return null;
                }

                if (Context.Users.Where(Usr => Usr.Email == email).Any())
                {
                    status = MembershipCreateStatus.DuplicateEmail;
                    return null;
                }

                User NewUser = new User
                {
                    Id = Guid.NewGuid(),
                    UserName = username,
                    Password = HashedPassword,
                    IsApproved = isApproved,
                    Email = email,
                    CreateDate = DateTime.UtcNow,
                    LastPasswordChangedDate = DateTime.UtcNow,
                    PasswordFailuresSinceLastSuccess = 0,
                    LastLoginDate = DateTime.UtcNow,
                    LastActivityDate = DateTime.UtcNow,
                    LastLockoutDate = DateTime.UtcNow,
                    IsLockedOut = false,
                    LastPasswordFailureDate = DateTime.UtcNow
                };

                Context.Users.Add(NewUser);
                Context.SaveChanges();
                status = MembershipCreateStatus.Success;
                return new MembershipUser(System.Web.Security.Membership.Provider.Name, NewUser.UserName, NewUser.Id, NewUser.Email, null, null, NewUser.IsApproved, NewUser.IsLockedOut, NewUser.CreateDate, NewUser.LastLoginDate, NewUser.LastActivityDate, NewUser.LastPasswordChangedDate, NewUser.LastLockoutDate);
            }
        }

        public override bool ValidateUser(string username, string password)
        {
            if (string.IsNullOrEmpty(username))
            {
                return false;
            }
            if (string.IsNullOrEmpty(password))
            {
                return false;
            }
            using (TazehaContext Context = new TazehaContext())
            {
                User User = null;
                User = Context.Users.FirstOrDefault(Usr => Usr.UserName == username);
                if (User == null)
                {
                    return false;
                }
                if (!User.IsApproved)
                {
                    return false;
                }
                if (User.IsLockedOut)
                {
                    return false;
                }
                String HashedPassword = User.Password;
                Boolean VerificationSucceeded = (HashedPassword != null && Crypto.VerifyHashedPassword(HashedPassword, password));
                if (VerificationSucceeded)
                {
                    User.PasswordFailuresSinceLastSuccess = 0;
                    User.LastLoginDate = DateTime.UtcNow;
                    User.LastActivityDate = DateTime.UtcNow;
                }
                else
                {
                    int? Failures = User.PasswordFailuresSinceLastSuccess;
                    if (Failures < MaxInvalidPasswordAttempts)
                    {
                        User.PasswordFailuresSinceLastSuccess += 1;
                        User.LastPasswordFailureDate = DateTime.UtcNow;
                    }
                    else if (Failures >= MaxInvalidPasswordAttempts)
                    {
                        User.LastPasswordFailureDate = DateTime.UtcNow;
                        User.LastLockoutDate = DateTime.UtcNow;
                        User.IsLockedOut = true;
                    }
                }
                Context.SaveChanges();
                if (VerificationSucceeded)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            if (string.IsNullOrEmpty(username))
            {
                return null;
            }
            using (TazehaContext Context = new TazehaContext())
            {
                User User = null;
                User = Context.Users.FirstOrDefault(Usr => Usr.UserName == username);
                if (User != null)
                {
                    if (userIsOnline)
                    {
                        User.LastActivityDate = DateTime.UtcNow;
                        Context.SaveChanges();
                    }
                    return new MembershipUser(System.Web.Security.Membership.Provider.Name, User.UserName, User.Id, User.Email, null, null, User.IsApproved, User.IsLockedOut, User.CreateDate, User.LastLoginDate, User.LastActivityDate, User.LastPasswordChangedDate, User.LastLockoutDate);
                }
                else
                {
                    return null;
                }
            }
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            if (providerUserKey is Guid) { }
            else
            {
                return null;
            }

            using (TazehaContext Context = new TazehaContext())
            {
                User User = null;
                User = Context.Users.Find(providerUserKey);
                if (User != null)
                {
                    if (userIsOnline)
                    {
                        User.LastActivityDate = DateTime.UtcNow;
                        Context.SaveChanges();
                    }
                    return new MembershipUser(System.Web.Security.Membership.Provider.Name, User.UserName, User.Id, User.Email, null, null, User.IsApproved, User.IsLockedOut, User.CreateDate, User.LastLoginDate, User.LastActivityDate, User.LastPasswordChangedDate, User.LastLockoutDate);
                }
                else
                {
                    return null;
                }
            }
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            if (string.IsNullOrEmpty(username))
            {
                return false;
            }
            if (string.IsNullOrEmpty(oldPassword))
            {
                return false;
            }
            if (string.IsNullOrEmpty(newPassword))
            {
                return false;
            }
            using (TazehaContext Context = new TazehaContext())
            {
                User User = null;
                User = Context.Users.FirstOrDefault(Usr => Usr.UserName == username);
                if (User == null)
                {
                    return false;
                }
                String HashedPassword = User.Password;
                Boolean VerificationSucceeded = (HashedPassword != null && Crypto.VerifyHashedPassword(HashedPassword, oldPassword));
                if (VerificationSucceeded)
                {
                    User.PasswordFailuresSinceLastSuccess = 0;
                }
                else
                {
                    int? Failures = User.PasswordFailuresSinceLastSuccess;
                    if (Failures < MaxInvalidPasswordAttempts)
                    {
                        User.PasswordFailuresSinceLastSuccess += 1;
                        User.LastPasswordFailureDate = DateTime.UtcNow;
                    }
                    else if (Failures >= MaxInvalidPasswordAttempts)
                    {
                        User.LastPasswordFailureDate = DateTime.UtcNow;
                        User.LastLockoutDate = DateTime.UtcNow;
                        User.IsLockedOut = true;
                    }
                    Context.SaveChanges();
                    return false;
                }
                String NewHashedPassword = Crypto.HashPassword(newPassword);
                if (NewHashedPassword.Length > 128)
                {
                    return false;
                }
                User.Password = NewHashedPassword;
                User.LastPasswordChangedDate = DateTime.UtcNow;
                Context.SaveChanges();
                return true;
            }
        }

        public override bool UnlockUser(string userName)
        {
            using (TazehaContext Context = new TazehaContext())
            {
                User User = null;
                User = Context.Users.FirstOrDefault(Usr => Usr.UserName == userName);
                if (User != null)
                {
                    User.IsLockedOut = false;
                    User.PasswordFailuresSinceLastSuccess = 0;
                    Context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public override int GetNumberOfUsersOnline()
        {
            DateTime DateActive = DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(Convert.ToDouble(System.Web.Security.Membership.UserIsOnlineTimeWindow)));
            using (TazehaContext Context = new TazehaContext())
            {
                return Context.Users.Where(Usr => Usr.LastActivityDate > DateActive).Count();
            }
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            if (string.IsNullOrEmpty(username))
            {
                return false;
            }
            using (TazehaContext Context = new TazehaContext())
            {
                User User = null;
                User = Context.Users.FirstOrDefault(Usr => Usr.UserName == username);
                if (User != null)
                {
                    Context.Users.Remove(User);
                    Context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public override string GetUserNameByEmail(string email)
        {
            using (TazehaContext Context = new TazehaContext())
            {
                User User = null;
                User = Context.Users.FirstOrDefault(Usr => Usr.Email == email);
                if (User != null)
                {
                    return User.UserName;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            MembershipUserCollection MembershipUsers = new MembershipUserCollection();
            using (TazehaContext Context = new TazehaContext())
            {
                totalRecords = Context.Users.Where(Usr => Usr.Email == emailToMatch).Count();
                IQueryable<User> Users = Context.Users.Where(Usr => Usr.Email == emailToMatch).OrderBy(Usrn => Usrn.UserName).Skip(pageIndex * pageSize).Take(pageSize);
                foreach (User user in Users)
                {
                    MembershipUsers.Add(new MembershipUser(System.Web.Security.Membership.Provider.Name, user.UserName, user.Id, user.Email, null, null, user.IsApproved, user.IsLockedOut, user.CreateDate, user.LastLoginDate, user.LastActivityDate, user.LastPasswordChangedDate, user.LastLockoutDate));
                }
            }
            return MembershipUsers;
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            MembershipUserCollection MembershipUsers = new MembershipUserCollection();
            using (TazehaContext Context = new TazehaContext())
            {
                totalRecords = Context.Users.Where(Usr => Usr.UserName == usernameToMatch).Count();
                IQueryable<User> Users = Context.Users.Where(Usr => Usr.UserName == usernameToMatch).OrderBy(Usrn => Usrn.UserName).Skip(pageIndex * pageSize).Take(pageSize);
                foreach (User user in Users)
                {
                    MembershipUsers.Add(new MembershipUser(System.Web.Security.Membership.Provider.Name, user.UserName, user.Id, user.Email, null, null, user.IsApproved, user.IsLockedOut, user.CreateDate, user.LastLoginDate, user.LastActivityDate, user.LastPasswordChangedDate, user.LastLockoutDate));
                }
            }
            return MembershipUsers;
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            MembershipUserCollection MembershipUsers = new MembershipUserCollection();
            using (TazehaContext Context = new TazehaContext())
            {
                totalRecords = Context.Users.Count();
                IQueryable<User> Users = Context.Users.OrderBy(Usrn => Usrn.UserName).Skip(pageIndex * pageSize).Take(pageSize);
                foreach (User user in Users)
                {
                    MembershipUsers.Add(new MembershipUser(System.Web.Security.Membership.Provider.Name, user.UserName, user.Id, user.Email, null, null, user.IsApproved, user.IsLockedOut, user.CreateDate, user.LastLoginDate, user.LastActivityDate, user.LastPasswordChangedDate, user.LastLockoutDate));
                }
            }
            return MembershipUsers;
        }

        #endregion

        #region Not Supported

        //CodeFirstMembershipProvider does not support password retrieval scenarios.
        public override bool EnablePasswordRetrieval
        {
            get { return false; }
        }
        public override string GetPassword(string username, string answer)
        {
            throw new NotSupportedException("Consider using methods from WebSecurity module.");
        }

        //CodeFirstMembershipProvider does not support password reset scenarios.
        public override bool EnablePasswordReset
        {
            get { return false; }
        }
        public override string ResetPassword(string username, string answer)
        {
            throw new NotSupportedException("Consider using methods from WebSecurity module.");
        }

        //CodeFirstMembershipProvider does not support question and answer scenarios.
        public override bool RequiresQuestionAndAnswer
        {
            get { return false; }
        }
        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotSupportedException("Consider using methods from WebSecurity module.");
        }

        //CodeFirstMembershipProvider does not support UpdateUser because this method is useless.
        public override void UpdateUser(MembershipUser user)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}