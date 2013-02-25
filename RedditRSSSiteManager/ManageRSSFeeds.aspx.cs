using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using RedditRSS;
using System.Configuration;

namespace RedditRSSSiteManager
{
    public partial class ManageRSSFeeds : System.Web.UI.Page
    {
        private string _managerCacheKey = "rssBotManagerCacheKey";

        public RedditRSSBotManager Manager
        {
            get
            {
                var manager = (RedditRSSBotManager)Cache[_managerCacheKey];
                if (manager == null)
                {
                    manager = new RedditRSSBotManager();
                    Cache[_managerCacheKey] = manager;
                }
                return manager;
            }
            set { Cache[_managerCacheKey] = value; }
        }

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
                var users = GetRedditUsers();
                ddlUsers.DataSource = users;
                ddlUsers.DataBind();

                Manager.AddBot("http://feeds.arstechnica.com/arstechnica/everything", "ArsTechnicaRSS", "thelehmanlip", "ocsid7", 5, BotMessageSent);
                //Manager.AddBot("http://www.rsspect.com/rss/qwantz.xml", "dinosaurcomics", "thelehmanlip", "ocsid7", 5, BotMessageSent);
            }
            //var bots = Manager.Bots;
            //gvBots.DataSource = bots;
            //gvBots.DataBind();
            if (Messages != null && Messages.Any())
            {
                var text = "<ul>";
                foreach (var x in Messages)
                    text += "<li>" + x + "</li>";
                text += "</ul>";
                lblMessages.Text = text;
            }
        }

        private List<RedditUser> GetRedditUsers()
        {
            var users = new List<RedditUser>();
            var allKeys = ConfigurationManager.AppSettings.AllKeys;
            bool tf = true;
            int counter = 1;
            while (tf)
            {
                var userKey = "RedditUser" + counter;
                var passwordKey = "RedditPassword" + counter;
                if (allKeys.Contains(userKey) || allKeys.Contains(passwordKey))
                {
                    var redditUser = new RedditUser() { UserName = ConfigurationManager.AppSettings[userKey], Password = ConfigurationManager.AppSettings[passwordKey] };
                    if (!string.IsNullOrEmpty(redditUser.Password) && !string.IsNullOrEmpty(redditUser.UserName))
                        users.Add(redditUser);
                }
                else
                    tf = false;
                counter++;
            }
            return users;
        }

        private class RedditUser
        {
            public string UserName { get; set; }
            public string Password { get; set; }
        }

        protected void BotMessageSent(object sender, MessageEventArgs e)
        {
            var msgs = Messages;
            if (msgs == null)
                msgs = new List<string>();
            msgs.Add(e.Message);
            Messages = msgs;
        }

        protected void btnAddFeed_Click(object sender, EventArgs e)
        {
            var feed = tbFeedUrl.Text;
            var subreddit = tbSubReddit.Text;
            var interval = int.Parse(tbInterval.Text);
            var user = GetRedditUsers().Where(x => x.UserName == ddlUsers.SelectedValue).FirstOrDefault();
            Manager.AddBot(feed, subreddit, user.UserName, user.Password, interval, BotMessageSent);
        }

        protected void gvBots_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var id = int.Parse((string)e.CommandArgument);
            if (e.CommandName == "StartBot")
            {
                Manager.StartBot(id);
            }
            else if (e.CommandName == "StopBot")
            {
                Manager.StopBot(id);
            }
            gvBots.DataBind();
        }

        // The return type can be changed to IEnumerable, however to support
        // paging and sorting, the following parameters must be added:
        //     int maximumRows
        //     int startRowIndex
        //     out int totalRowCount
        //     string sortByExpression
        public IQueryable<RedditRSS.RedditRSSBot> gvBots_GetData()
        {
            return Manager.Bots.AsQueryable();
        }
    }
}