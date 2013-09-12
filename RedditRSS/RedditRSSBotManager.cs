using RedditRSS.Data;
using RedditRSS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditRSS
{
    public class RedditRSSBotManager
    {
        private static RedditRSSBotManager _instance;
        public static RedditRSSBotManager Instance
        {
            get
            {
                if (_instance == null)
                    Instance = new RedditRSSBotManager();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        public RedditRSSBotManager()
        {
            Init();
        }
        public void Init()
        {
            //intantiate all the bots from the db?
        }
        public void InitAll()
        {
            var ctx = new RedditRSSEntities();
            foreach (var appUser in ctx.AppUsers)
            {
                InitUser(appUser.ID);
            }
        }
        public void InitUser(int appUserID)
        {
            var ctx = new RedditRSSEntities();
            foreach (var botData in ctx.RedditRSSBotDatas.Where(x => x.RedditUser.AppUserID == appUserID))
            {
                if (!Bots.Any(x => x.ID == botData.ID))
                    AddBot(botData);
            }
        }

        private List<RedditRSSBot> _bots = new List<RedditRSSBot>();
        public List<RedditRSSBot> Bots
        {
            get { return _bots; }
            set { _bots = value; }
        }

        public IEnumerable<RedditRSSBot> GetBots(int appUserID)
        {
            return Bots.Where(x => x.AppUserID == appUserID);
        }

        public List<RedditRSSBot> RunningBots
        {
            get { return Bots.Where(x => x.Started).ToList(); }
        }

        public RedditRSSBot AddBot(RedditRSSBotData botData, EventHandler<MessageEventArgs> messageHandler = null)
        {
            var bot = new RedditRSSBot();
            bot.RedditRSSBotData = botData;
            Bots.Add(bot);

            if (messageHandler != null)
                bot.SendMessage += messageHandler;

            return bot;
        }

        public void Start(RedditRSSBot bot)
        {
            bot.Start();
        }

        public void StartBot(int botID)
        {
            var bot = Bots.Where(x => x.RedditRSSBotData.ID == botID && x.CurrentStatus != RedditRSSBot.BotStatus.Started).FirstOrDefault();
            if (bot != null)
            {
                bot.Start();
            }
        }

        public void StopAllBots()
        {
            Bots.Where(x => x.CurrentStatus == RedditRSSBot.BotStatus.Started).ForEach(x => x.Stop());
        }

        public void StopBot(int id)
        {
            var bot = Bots.Where(x => x.RedditRSSBotData.ID == id && x.CurrentStatus == RedditRSSBot.BotStatus.Started).FirstOrDefault();
            if (bot != null)
                bot.Stop();
        }

        public void StartAllBots()
        {
            Bots.Where(x => !x.Started).ForEach(x => x.Start());
        }

    }
}
