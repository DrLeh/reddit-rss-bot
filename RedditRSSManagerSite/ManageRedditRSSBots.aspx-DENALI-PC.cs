using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using RedditRSS;
using System.Configuration;
using RedditRSS.Data;

namespace RedditRSSManagerSite
{
    public partial class ManageRedditRSSBots : BasePage
    {


        private string _messageCacheKey = "messagesCacheKey";
        public List<string> Messages
        {
            get
            {
                var t = (List<string>)Cache[_messageCacheKey];
                if (t != null && !t.Any())
                    t = new List<string>();
                return t;
            }
            set { Cache[_messageCacheKey] = value; }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!Page.IsPostBack)
            {
                if (!UserLoggedIn)
                    Response.Redirect("~/CustomAuth/Login.aspx?returnUrl=ManageRedditRSSBots.aspx");
                else
                    ucBotList.DataBind();
                //Manager.AddBot("http://feeds.arstechnica.com/arstechnica/everything", "ArsTechnicaRSS", "", "", 5, BotMessageSent);
                //Manager.AddBot("http://www.rsspect.com/rss/qwantz.xml", "dinosaurcomics", "", "", 5, BotMessageSent);

            }
        }

        private List<RedditUser> GetRedditUsers()
        {
            var ctx = new RedditRSSEntities();
            return ctx.RedditUsers.ToList();
        }
        protected void btnAddFeed_Click(object sender, EventArgs e)
        {
            var feed = tbFeedUrl.Text;
            var subreddit = tbSubReddit.Text;
            var interval = int.Parse(tbInterval.Text);

            var ctx = new RedditRSSEntities();
            var selectedUserID = int.Parse(ddlUsers.SelectedValue);
            var user = ctx.RedditUsers.Where(x => x.ID == selectedUserID).FirstOrDefault();
            var botData = new RedditRSSBotData()
            {
                FeedUrl = feed,
                Subreddit = subreddit,
                RedditUser = user,
                Interval = interval
            };
            ctx.RedditRSSBotDatas.Add(botData);
            ctx.SaveChanges();

            var manager = RedditRSSBotManager.Instance;
            manager.AddBot(botData.CreateBot());
            ucBotList.DataBind();
        }

        public IQueryable<RedditUser> ddlUsers_GetData()
        {
            var ctx = new RedditRSSEntities();
            if(UserLoggedIn)
                return ctx.RedditUsers.Where(x => x.AppUserID == CurrentUserID).AsQueryable();
            return null;
        }
    }
}