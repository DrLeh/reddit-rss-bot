using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RedditRSS.Data;

namespace RedditRSS
{
    public static class RedditRSSBotManager
    {
        private static void ExecuteBotInternal(RedditRSSEntities ctx, int id)
        {
            var botData = ctx.RedditRSSBotDatas.Where(x => x.ID == id).FirstOrDefault();
            if (botData != null)
            {
                var bot = new RedditRSSBot(botData);
                var newSubmissions = bot.CheckAndSubmitNews();
                newSubmissions.ToList().ForEach(x => ctx.RedditSubmissions.Add(x));
            }
        }
        public static void ExecuteAllRunningBots()
        {
            var ctx = new RedditRSSEntities();
            var botDatas = ctx.RedditRSSBotDatas.Where(x => x.StatusTypeID == 2).Select(x => x.ID);
            foreach (var botDataID in botDatas)
            {
                ExecuteBotInternal(ctx, botDataID);
            }
            ctx.SaveChanges();
        }

        public static void ExecuteBot(int id)
        {
            var ctx = new RedditRSSEntities();
            ExecuteBotInternal(ctx, id);
            ctx.SaveChanges();
        }


    }
}
