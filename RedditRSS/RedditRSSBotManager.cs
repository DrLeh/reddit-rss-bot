using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditRSS
{
    public class RedditRSSBotManager
    {

        public RedditRSSBotManager()
        {

        }
        public RedditRSSBotManager(List<RedditRSSBot> bots)
        {
            Bots = bots;
        }

        private List<RedditRSSBot> _bots = new List<RedditRSSBot>();
        public List<RedditRSSBot> Bots
        {
            get { return _bots; }
            set { _bots = value; }
        }


        public RedditRSSBot AddBot(string feedUrl, string subreddit, string redditUsername, string redditPassword, int minutesToCheck, EventHandler<MessageEventArgs> messageHandler)
        {
            var bot = AddBot(feedUrl, subreddit, redditUsername, redditPassword, minutesToCheck);
            if (bot != null)
            {
                bot.SendMessage += messageHandler;
            }
            return bot;
        }

        public RedditRSSBot AddBot(string feedUrl, string subreddit, string redditUsername, string redditPassword, int minutesToCheck = 5)
        {
            int id = 1;
            if(Bots.Any())
                id = Bots.Max(x => x.ID) + 1;

            RedditRSSBot bot = null;
            if (Bots.Any(x => x.FeedUrl == feedUrl && x.Subreddit == subreddit)) return bot;

            bot = new RedditRSSBot() { ID = id, FeedUrl = feedUrl, Interval = minutesToCheck, Subreddit = subreddit, Username = redditUsername, Password = redditPassword }; 
            Bots.Add(bot);

            return bot;
        }

        public RedditRSSBot Start(RedditRSSBot bot)
        {
            bot.Start();
            return bot;
        }

        public void StartBot(int botID)
        {
            var bot = Bots.Where(x => x.ID == botID).FirstOrDefault();
            if (bot != null && bot.CurrentStatus != RedditRSSBot.BotStatus.Started)
            {
                bot.Start();
            }
        }
        public void StopAllBots()
        {
            Bots.ForEach(bot =>
                {
                    if (bot.CurrentStatus == RedditRSSBot.BotStatus.Started)
                    {
                        bot.Stop();
                    }
                });
        }
        public void StopBot(int id)
        {
            foreach (var bot in Bots.Where(x => x.ID == id))
            {
                if (bot.CurrentStatus == RedditRSSBot.BotStatus.Started)
                {
                    bot.Stop();
                }
            }
        }

        public RedditRSSBot AddAndStartBot(string feedUrl, string subreddit, string redditUsername, string redditPassword, int minutesToCheck, EventHandler<MessageEventArgs> messageHandler)
        {
            return Start(AddBot(feedUrl, subreddit, redditUsername, redditPassword, minutesToCheck, messageHandler));
        }
        public RedditRSSBot AddAndStartBot(string feedUrl, string subreddit, string redditUsername, string redditPassword, int minutesToCheck = 5)
        {
            return Start(AddBot(feedUrl, subreddit, redditUsername, redditPassword, minutesToCheck));
        }

        public void StartAllBots()
        {
            foreach (var x in Bots.Where(x => !x.Started))
                x.Start();
        }

    }
}
