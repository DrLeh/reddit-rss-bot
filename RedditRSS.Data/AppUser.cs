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
    
    public partial class AppUser
    {
        public AppUser()
        {
            this.RedditUsers = new HashSet<RedditUser>();
        }
    
        public int ID { get; set; }
        public string Username { get; set; }
        public string HashedPassword { get; set; }
    
        public virtual ICollection<RedditUser> RedditUsers { get; set; }
    }
}
