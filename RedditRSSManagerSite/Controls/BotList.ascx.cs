using RedditRSS;
using RedditRSS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RedditRSSManagerSite.Controls
{
    public partial class BotList : BaseControl
    {
        public enum BotListDisplayMode
        {
            SingleUser,
            CurrentlyRunning,
            All
        }

        public RedditRSSBotManager Manager
        {
            get { return RedditRSSBotManager.Instance; }
        }

        public BotListDisplayMode DisplayMode { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if(DisplayMode == BotListDisplayMode.SingleUser)
                    if(UserLoggedIn)
                        Manager.InitUser((int)CurrentUserID);
                if (DisplayMode == BotListDisplayMode.All)
                    Manager.InitAll();
            }
        }

        private IEnumerable<RedditRSSBot> GetBots(int appUserID)
        {
            return Manager.GetBots(appUserID);
        }

        protected void gvBots_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var id = int.Parse((string)e.CommandArgument);
            var ctx = new RedditRSSEntities();

            if (e.CommandName == "StartBot")
            {
                Manager.StartBot(id);
            }
            else if (e.CommandName == "StopBot")
            {
                Manager.StopBot(id);
            }
            else if (e.CommandName == "DeleteBot")
            {
                Manager.Bots.RemoveAll(x => x.ID == id);
                var botToRemove = ctx.RedditRSSBotDatas.Where(x => x.ID == id).FirstOrDefault();
                ctx.RedditRSSBotDatas.Remove(botToRemove);
                ctx.SaveChanges();
            }
            gvBots.DataBind();
        }

        public IQueryable<RedditRSSBot> gvBots_GetData()
        {
            if (this.DisplayMode == BotListDisplayMode.SingleUser)
            {
                return Manager.GetBots((int)CurrentUserID).AsQueryable();
            }
            else if (DisplayMode == BotListDisplayMode.CurrentlyRunning || DisplayMode == BotListDisplayMode.All)
            {
                return Manager.Bots.AsQueryable();
            }
            else
                return null;
        }
    }
}