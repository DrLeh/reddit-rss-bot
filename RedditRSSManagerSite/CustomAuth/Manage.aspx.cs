using RedditRSS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RedditRSSManagerSite.CustomAuth
{
    public partial class Manage : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!UserLoggedIn)
            {
                Response.Redirect("~/");
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            var userID = (int)CurrentUserID;
            var ctx = new RedditRSSEntities();
            var appUser = ctx.AppUsers.Where(x => x.ID == userID).FirstOrDefault();
            var redditUsers = ctx.RedditUsers.Where(x => x.AppUserID == userID).ToList();
            var bots = ctx.RedditRSSBotDatas.Where(x => redditUsers.Any(y => x.RedditUserID == y.ID)).ToList();
            bots.ForEach(x => ctx.RedditRSSBotDatas.Remove(x));
            redditUsers.ForEach(x => ctx.RedditUsers.Remove(x));
            ctx.AppUsers.Remove(appUser);
            ctx.SaveChanges();
            Response.Redirect("~/");
        }
    }
}