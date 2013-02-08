using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Net;
using System.ServiceModel.Syndication;
using System.Web;
using Newtonsoft.Json;
using System.Threading;

using RedditRSS;

namespace RSSBotCS
{
    class Program
    {
        static void Main(string[] args)
        {
            var bot = RedditRSSBotManager.AddAndStartBot("http://feeds.arstechnica.com/arstechnica/everything", "ArsTechnicaRSS", "thelehmanlip", "ocsid7",  5, bot_SendMessage);
        }

        static void bot_SendMessage(object sender, MessageEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
    }
}

