using RedditRSS.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RedditRSSManagerSite.CustomAuth
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!bool.Parse(ConfigurationManager.AppSettings["EnableRegistering"]))
                {
                    Response.Redirect("~/");
                }
            }
        }
        protected void btnRegister_Click(object sender, EventArgs e)
        {
            var username = tbUsername.Text;
            var password = tbPassword.Text;
            var passwordConfirm = tbPasswordConfirm.Text;
            if (username == null && password == null && passwordConfirm == null)
            {
                lblMessage.Text = "All fields are required";
            }
            else if (password != passwordConfirm)
            {
                lblMessage.Text = "Password Fields don't match!";
            }
            else
            {
                var ctx = new RedditRSSEntities();
                var appUser = new AppUser();
                appUser.Username = username;
                appUser.Password = password;
                appUser.HashPassword();
                ctx.AppUsers.Add(appUser);
                ctx.SaveChanges();
                Session[AppConstants.SessionKeys.LOGGED_IN_USER_ID] = appUser.ID;
                Session[AppConstants.SessionKeys.LOGGED_IN_USER_NAME] = appUser.Username;
                Response.Redirect("~/");
            }
        }
    }
}