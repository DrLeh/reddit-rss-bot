//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RedditRSS.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class RedditRSSBotStatusType
    {
        public RedditRSSBotStatusType()
        {
            this.RedditRSSBotDatas = new HashSet<RedditRSSBotData>();
        }
    
        public int ID { get; set; }
        public string Description { get; set; }
    
        public virtual ICollection<RedditRSSBotData> RedditRSSBotDatas { get; set; }
    }
}
