using RedditRSS;
using RedditRSS.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditRSS.Data
{
    public partial class RedditUser
    {
        string _password = "";
        public string Password
        {
            get
            {
                if (!string.IsNullOrEmpty(_password))
                    return _password;
                if (!string.IsNullOrEmpty(this.HashedPassword))
                    return CryptUtil.DecryptRedditUserPassword(this.HashedPassword, ConfigurationManager.AppSettings["CryptSharedSecret"]);
                return null;
            }
            set
            {
                _password = value;
                HashedPassword = EncryptPassword();
            }
        }

        public string EncryptPassword()
        {
            if (string.IsNullOrEmpty(_password))
                return null;
            return CryptUtil.EncryptRedditPassword(_password, ConfigurationManager.AppSettings["CryptSharedSecret"]);
        }
    }

    public partial class AppUser
    {
        public string Password { get; set; }
        public string HashPassword()
        {
            if (Password == null)
                throw new ArgumentException("Password is null, cannot hash.");
            if (this.HashedPassword == null)
                HashedPassword = CryptUtil.HashAppUserPassword(Password);
            return HashedPassword;
        }
    }

    //public partial class RedditRSSBotData
    //{
    //    public RedditRSSBot CreateBot()
    //    {
    //        return new RedditRSSBot() { RedditRSSBotData = this };
    //    }
    //}
}
