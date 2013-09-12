using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace RedditRSSManagerSite
{
    public class BaseControl : UserControl
    {
        public int? CurrentUserID { get { return (int?)Session[AppConstants.SessionKeys.LOGGED_IN_USER_ID]; } }
        public string CurrentUserName { get { return (string)Session[AppConstants.SessionKeys.LOGGED_IN_USER_NAME]; } }
        public bool UserLoggedIn { get { return CurrentUserID != null; } }
    }
}