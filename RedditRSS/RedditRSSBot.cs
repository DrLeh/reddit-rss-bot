using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace RedditRSS
{
    public class RedditRSSBot
    {

        #region Nested Classes

        public class RedditLoginData
        {
            public string modhash;
            public string cookie;
        }

        public class RedditSubmissionData
        {
            public string Title { get; set; }
            public string Url { get; set; }
            public string SubReddit { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
        }

        #endregion


        #region Properties

        public enum BotStatus
        {
            NotStarted,
            Started,
            Stopped
        }
        public BotStatus CurrentStatus { get; set; }

        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FeedUrl { get; set; }
        public string Subreddit { get; set; }

        private int _minutesToCheck = 5;

        public int Interval
        {
            get { return _minutesToCheck; }
            set { _minutesToCheck = value; }
        }

        public bool Started { get; set; }

        #endregion 


        #region Events

        public event EventHandler<MessageEventArgs> SendMessage;

        #endregion


        #region Constructors

        public RedditRSSBot()
        {

        }

        #endregion


        #region Methods

        private Thread ExecutionThread;

        public void Start()
        {
            CurrentStatus = BotStatus.Started;
            ExecutionThread = new Thread(new ThreadStart(WatchFeed));
            ExecutionThread.Start();
        }

        public void Stop()
        {
            CurrentStatus = BotStatus.Stopped;
            ExecutionThread.Abort();
        }

        public void WatchFeed()
        {
            string lastLinkSubmitted = null;
            while (true)
            {
                var rsd = Read();
                if (rsd != null && (lastLinkSubmitted == null || rsd.Url != lastLinkSubmitted))
                {
                    Submit(rsd);
                    lastLinkSubmitted = rsd.Url;
                }
                else if (lastLinkSubmitted != null && lastLinkSubmitted == rsd.Url)
                {
                    OnSendMessage("This link submitted already.");
                }
                Thread.Sleep(Interval * 60 * 1000);
            }
        }

        public RedditSubmissionData Read()
        {
            RedditSubmissionData rsd = null;
            try
            {
                SyndicationFeed feed = SyndicationFeed.Load(XmlReader.Create(FeedUrl));
                var item = feed.Items.OrderByDescending(x => x.PublishDate).FirstOrDefault();
                if (item != null)
                {
                    rsd = new RedditSubmissionData();
                    rsd.Title = item.Title.Text;
                    var link = item.Links.FirstOrDefault();
                    rsd.Url = link != null ? link.Uri.ToString() : null;
                    rsd.SubReddit = Subreddit;
                    rsd.Username = Username;
                    rsd.Password = Password;
                }
            }
            catch (Exception x)
            {
                OnSendMessage("Error reading feed: " + FeedUrl);
            }
            return rsd;
        }


        public const string REDDIT_URI = "http://www.reddit.com/api/submit";

        public RedditLoginData Login(string username, string password)
        {
            var uri = "http://www.reddit.com/api/login/" + username;
            var parameters = string.Format("user={0}&passwd={1}&api_type=json", username, password);
            var response = HttpPost(uri, parameters);
            var loginData = new RedditLoginData();
            if (response.Contains("modhash"))
            {
                //example response from a successful login
                //"{\"json\": {\"errors\": [], \"data\": {\"modhash\": \"5kdh1o3k950c5e71ff91152eea871995a12922c7b2e873e951\", \"cookie\": \"15777892,2012-10-23T22:16:39,3637e3b20aa62fe0c1fc974f6f54e0b14e982441\"}}}"
                var index = response.IndexOf("{\"modhash\":");
                var sub = response.Substring(index, response.Length - index - 2);
                loginData = JsonConvert.DeserializeObject<RedditLoginData>(sub);
            }
            return loginData;
        }

        public void Submit(RedditSubmissionData rsd)
        {
            Submit(rsd.Title, rsd.Url, rsd.SubReddit, rsd.Username, rsd.Password);
        }

        public void Submit(string title, string url, string subreddit, string username, string password)
        {
            var loginData = Login(username, password);
            if (!string.IsNullOrEmpty(loginData.modhash))
            {
                var parameters = "";
                parameters += "kind=link";
                parameters += "&url=" + HttpUtility.UrlEncode(url);
                parameters += "&title=" + HttpUtility.UrlEncode(title);
                parameters += "&sr=" + HttpUtility.UrlEncode(subreddit);
                parameters += "&r=" + HttpUtility.UrlEncode(subreddit);
                parameters += "&uh=" + HttpUtility.UrlEncode(loginData.modhash);

                var cookies = new CookieContainer();
                var cookie = new Cookie("reddit_session", HttpUtility.UrlEncode(loginData.cookie), "/api/submit", "reddit.com");
                cookie.Secure = false;
                cookie.Expires = DateTime.MinValue;
                cookies.Add(cookie);

                var tryAgainMessageReceieved = true;
                string response = null;
                while (tryAgainMessageReceieved)
                {
                    response = HttpPost(REDDIT_URI, parameters, cookies);
                    if (response.Contains("try again in"))
                    {
                        tryAgainMessageReceieved = true;
                        //this line yet untested
                        //var tryAgainMinutes = int.Parse(response.Substring(response.IndexOf("try again in "), 14));
                        var tryAgainMinutes = 3;
                        Thread.Sleep(tryAgainMinutes * 60 * 1000);
                        OnSendMessage("Trying to submit too much, trying again in " + tryAgainMinutes + " minutes");
                    }
                    if (response.Contains("that link has already been submitted"))
                    {
                        tryAgainMessageReceieved = false;
                        OnSendMessage("This link has already been submitted: /r/" + subreddit + " Title: " + title + " url: " + url);
                    }
                    else
                    {
                        tryAgainMessageReceieved = false;
                        OnSendMessage("Post submitted: /r/" + subreddit + " Title: " + title + " url: " + url);
                    }
                }
            }
            else
            {
                OnSendMessage("Login failed");
            }
        }

        public string HttpPost(string uri, string parameters)
        {
            return HttpPost(uri, parameters, null);
        }

        public string HttpPost(string uri, string parameters, CookieContainer cookies)
        {
            var req = (HttpWebRequest)HttpWebRequest.Create(uri);
            req.ContentType = "application/x-www-form-urlencoded";
            req.Method = "POST";
            if (cookies != null)
                req.CookieContainer = cookies;
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(parameters);
            req.ContentLength = bytes.Length;
            System.IO.Stream os = req.GetRequestStream();
            os.Write(bytes, 0, bytes.Length);
            os.Close();
            System.Net.WebResponse resp = req.GetResponse();
            if (resp == null) return null;
            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
            return sr.ReadToEnd().Trim();
        }

        public void OnSendMessage(string message)
        {
            if (SendMessage != null)
                SendMessage(this, new MessageEventArgs(message));
        }

        #endregion
    }

}
