using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using System.Security.Cryptography;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using Tazeyab.Common.Models;
using Tazeyab.Common.ViewModels;
using Tazeyab.Web.Models;

namespace Tazeyab.Web.Controllers
{
    public class MembershipService : IMembershipService
    {
        public Guid StringToGUID(string value)
        {
            // Create a new instance of the MD5CryptoServiceProvider object.
            MD5 md5Hasher = MD5.Create();
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(value));
            return new Guid(data);
        }
        public MembershipCreateStatus CreateUser(string userName, string password,
                                        string email, string OpenID)
        {
            if (String.IsNullOrEmpty(userName)) throw
            new ArgumentException("Value cannot be null or empty.", "userName");
            if (String.IsNullOrEmpty(password)) throw
            new ArgumentException("Value cannot be null or empty.", "password");
            if (String.IsNullOrEmpty(email)) throw
            new ArgumentException("Value cannot be null or empty.", "email");

            MembershipCreateStatus status;
            //_provider.CreateUser(userName, password, email, null, null, true,
            //                     StringToGUID(OpenID), out status);
            System.Web.Security.Membership.CreateUser(userName, password, email, null, null, true,
                                 StringToGUID(OpenID), out status);
            return status;
        }
        public MembershipUser GetUser(string OpenID)
        {
            //DotNetOpenAuth.OAuth.Web
            var fetch = new FetchRequest();
            fetch.Attributes.AddRequired("http://axschema.org/contact/country/home");
            fetch.Attributes.AddRequired("http://axschema.org/contact/email");
            fetch.Attributes.AddRequired("http://axschema.org/namePerson/first");
            fetch.Attributes.AddRequired("http://axschema.org/namePerson/last");
            fetch.Attributes.AddRequired("http://axschema.org/pref/language");
            fetch.Attributes.AddRequired("http://schemas.openid.net/ax/api/user_id");
            return null;// _provider.GetUser(StringToGUID(OpenID), true);
        }

        public int MinPasswordLength
        {
            get { throw new NotImplementedException(); }
        }

        public bool ValidateUser(string userName, string password)
        {
            throw new NotImplementedException();
        }



        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }


    }
}
