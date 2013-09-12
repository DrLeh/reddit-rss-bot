using RedditRSS.Data;
using RedditRSS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RedditRSSManagerSite.CustomAuth
{
    public partial class Login : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (UserLoggedIn)
            {
                Response.Redirect("~/");
            }

            if (!Page.IsPostBack)
            {
                ScriptManager.GetCurrent(this.Page).SetFocus(tbUsername);
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            var returnUrl = Request.QueryString["returnUrl"];
            bool loginValid = false;
            var userName = tbUsername.Text;
            var pw = tbPassword.Text;
            var ctx = new RedditRSSEntities();
            var appUser = ctx.AppUsers.Where(x => x.Username == userName).FirstOrDefault();
            if (appUser != null)
            {
                var validPW = CryptUtil.ValidatePassword(pw, appUser.HashedPassword);
                if (validPW)
                {
                    loginValid = true;
                    Session[AppConstants.SessionKeys.LOGGED_IN_USER_ID] = appUser.ID;
                    Session[AppConstants.SessionKeys.LOGGED_IN_USER_NAME] = appUser.Username;
                }
            }
            if (!loginValid)
            {
                lblMessage.Text = "Login invalid. Please check the username and password";
            }
            else
                Response.Redirect("~/" + (returnUrl == null ? "": returnUrl));
        }
    }
}