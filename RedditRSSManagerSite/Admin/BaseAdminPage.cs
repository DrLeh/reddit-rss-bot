using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RedditRSSManagerSite.Admin
{
    public class BaseAdminPage : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!UserLoggedIn)
            {
                var path = Request.Path;
                path = path.Substring(1, path.Length - 1);
                Response.Redirect("~/CustomAuth/Login.aspx?returnUrl="+ path);
            }
            if (CurrentUserName != "admin")
            {
                Response.Redirect("~/");
            }
        }
    }
}