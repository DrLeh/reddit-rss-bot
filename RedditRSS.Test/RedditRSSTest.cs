using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RedditRSS;

namespace RedditRSS.Test
{
    public class RedditRSSTest
    {
        static void Main(string[] args)
        {
            var startTime = DateTime.Now;
            Console.WriteLine("Executing all running bots");
            RedditRSSBotManager.ExecuteAllRunningBots();
            var endTime = DateTime.Now;
            var diff = endTime.Subtract(startTime);
            Console.WriteLine("Done in " + diff.TotalMilliseconds + " ms");
            Console.ReadKey();
        }
    }
}
