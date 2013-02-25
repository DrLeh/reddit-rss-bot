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

        public class RedditSubmission
        {
            public string Title { get; set; }
            public string Url { get; set; }
            public string SubReddit { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }

            public bool Equals(RedditSubmission b)
            {
                return this.Title == b.Title
                    && this.SubReddit == b.SubReddit
                    && this.Url == b.SubReddit;
            }
        }

        #endregion


        #region Properties

        public const string REDDIT_URI = "http://www.reddit.com/api/submit";

        public enum BotStatus
        {
            NotStarted,
            Started,
            Stopped
        }

        public BotStatus CurrentStatus { get; set; }
        public bool Started
        {
            get
            {
                return CurrentStatus == BotStatus.Started;
            }
        }

        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FeedUrl { get; set; }
        public string Subreddit { get; set; }

        private int _interval = 5;
        public int Interval
        {
            get { return _interval; }
            set { _interval = value; }
        }

        private int _submissionsToStore = 10;
        public int SubmissionsToStore
        {
            get { return _submissionsToStore; }
            set { _submissionsToStore = value; }
        }

        public Queue<RedditSubmission> LastSubmitted
        {
            get;
            set;
        }

        #endregion


        #region Events

        public event EventHandler<MessageEventArgs> SendMessage;

        #endregion


        #region Constructors

        public RedditRSSBot()
        {
            LastSubmitted = new Queue<RedditSubmission>();
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

        public const int READ_COUNT = 10;
        public void WatchFeed()
        {
            while (true)
            {
                var rsds = Read(READ_COUNT);
                foreach (var rsd in rsds)
                {
                    Submit(rsd);
                }
                Thread.Sleep(Interval * 60 * 1000);
            }
        }

        public List<RedditSubmission> Read(int readCount = 1)
        {
            var list = new List<RedditSubmission>();
            try
            {
                SyndicationFeed feed = SyndicationFeed.Load(XmlReader.Create(FeedUrl));
                var items = feed.Items.OrderByDescending(x => x.PublishDate).Take(readCount);
                foreach(var item in items)
                {
                    var rs = new RedditSubmission();
                    rs.Title = item.Title.Text;
                    var link = item.Links.FirstOrDefault();
                    rs.Url = link != null ? link.Uri.ToString() : null;
                    rs.SubReddit = Subreddit;
                    rs.Username = Username;
                    rs.Password = Password;
                    list.Add(rs);
                }
            }
            catch (Exception)
            {
                OnSendMessage("Error reading feed: " + FeedUrl);
            }
            return list;
        }

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

        public void Submit(string title, string url, string subreddit, string username, string password)
        {
            Submit(new RedditSubmission() { Title = title, Url = url, SubReddit = subreddit, Username = username, Password = password });
        }

        public void Submit(RedditSubmission rs)
        {
            //submit if not already submitted
            if (!LastSubmitted.Any(x => x.Equals(rs)))
            {
                //need to try storing this instead of doing it on every submit. might need to do it anyway though.
                var loginData = Login(rs.Username, rs.Password);
                if (!string.IsNullOrEmpty(loginData.modhash))
                {
                    var parameters = "";
                    parameters += "kind=link";
                    parameters += "&url=" + HttpUtility.UrlEncode(rs.Url);
                    parameters += "&title=" + HttpUtility.UrlEncode(rs.Title);
                    parameters += "&sr=" + HttpUtility.UrlEncode(rs.SubReddit);
                    parameters += "&r=" + HttpUtility.UrlEncode(rs.SubReddit);
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
                        else if (response.Contains("that link has already been submitted"))
                        {
                            tryAgainMessageReceieved = false;
                            OnSendMessage("This link has already been submitted: /r/" + rs.SubReddit + " Title: " + rs.Title + " url: " + rs.Url);
                        }
                        else
                        {
                            tryAgainMessageReceieved = false;
                            OnSendMessage("Post submitted: /r/" + rs.SubReddit + " Title: " + rs.Title + " url: " + rs.Url);
                        }
                    }
                    LastSubmitted.Enqueue(rs);
                    if (LastSubmitted.Count > SubmissionsToStore)
                        LastSubmitted.Dequeue();
                }
                else
                {
                    OnSendMessage("Login failed");
                }
            }
            else
            {
                OnSendMessage("Link already submitted.");
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
