using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditRSS
{
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
}
