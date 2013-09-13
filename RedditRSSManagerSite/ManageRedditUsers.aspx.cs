using RedditRSS;
using RedditRSS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RedditRSSManagerSite
{
    public partial class ManageRedditUsers : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!UserLoggedIn)
                Response.Redirect("~/CustomAuth/Login.aspx?returnUrl=ManageRedditUsers.aspx");
        }

        // The return type can be changed to IEnumerable, however to support
        // paging and sorting, the following parameters must be added:
        //     int maximumRows
        //     int startRowIndex
        //     out int totalRowCount
        //     string sortByExpression
        public IQueryable<RedditRSS.Data.RedditUser> gvUsers_GetData()
        {
            var ctx = new RedditRSSEntities();
            if (UserLoggedIn)
                return ctx.RedditUsers.Where(x => x.AppUserID == CurrentUserID);
            return null;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            var user = new RedditUser();
            user.Username = tbUsername.Text;
            user.Password = tbPassword.Text;
            user.AppUserID = CurrentUserID;
            var ctx = new RedditRSSEntities();
            ctx.RedditUsers.Add(user);
            ctx.SaveChanges();
            gvUsers.DataBind();
        }

        protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteUser")
            {
                var id = int.Parse((string)e.CommandArgument);
                var ctx = new RedditRSSEntities();
                var user = ctx.RedditUsers.Where(x => x.ID == id).FirstOrDefault();
                var botDatas = ctx.RedditRSSBotDatas.Where(x => x.RedditUserID == id).ToList();

                var botsToRemove = ctx.RedditRSSBotDatas.Where(x => x.RedditUserID == id).ToList();
                
                botDatas.ForEach(x => ctx.RedditRSSBotDatas.Remove(x));
                ctx.RedditUsers.Remove(user);
                ctx.SaveChanges();
                gvUsers.DataBind();
            }
        }

    }
}