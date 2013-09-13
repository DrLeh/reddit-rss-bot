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

        public BotListDisplayMode DisplayMode { get; set; }
        
        protected void gvBots_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var id = int.Parse((string)e.CommandArgument);
            var ctx = new RedditRSSEntities();

            if (e.CommandName == "StartBot")
            {
                var bot = ctx.RedditRSSBotDatas.Where(x => x.ID == id).FirstOrDefault();
                if (bot != null)
                    bot.StatusTypeID = 2;
                ctx.SaveChanges();
            }
            else if (e.CommandName == "StopBot")
            {
                var bot = ctx.RedditRSSBotDatas.Where(x => x.ID == id).FirstOrDefault();
                if (bot != null)
                    bot.StatusTypeID = 3;
                ctx.SaveChanges();
            }
            else if (e.CommandName == "DeleteBot")
            {
                var botToRemove = ctx.RedditRSSBotDatas.Where(x => x.ID == id).FirstOrDefault();
                ctx.RedditRSSBotDatas.Remove(botToRemove);
                ctx.SaveChanges();
            }
            gvBots.DataBind();
        }

        public IQueryable<RedditRSSBotData> gvBots_GetData()
        {
            var ctx = new RedditRSSEntities();
            var results = new List<RedditRSSBotData>().AsQueryable();
            if (this.DisplayMode == BotListDisplayMode.SingleUser)
            {
                results = ctx.RedditRSSBotDatas.Where(x => x.RedditUser.AppUserID == (int)CurrentUserID);
            }
            else if (DisplayMode == BotListDisplayMode.All)
            {
                results = ctx.RedditRSSBotDatas;
            }
            else if (DisplayMode == BotListDisplayMode.CurrentlyRunning)
            {
                results = ctx.RedditRSSBotDatas.Where(x => x.StatusTypeID == 2);
            }
            return results;
        }
    }
}