﻿using RedditRSS;
using RedditRSS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RedditRSSManagerSite.Admin
{
    public partial class CurrentlyRunningBots : BaseAdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(this, e);
        }
    }
}