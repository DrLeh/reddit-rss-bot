<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="RedditRSSManagerSite._Default" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    <section class="featured">
    </section>
</asp:Content>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h2>Home Page</h2>
    <p>Welcome to the crappy first version of the RSS -> reddit site thingy! This site allows you to take any RSS feed and have new items automatically posted to a subreddit!</p>
    <p>To set up a bot, here's what you'll need to do:</p>
    <ol>
        <li>Register for an account on this site.</li>
        <li>Once logged in, navigate to "Manage Reddit Users"</li>
        <li>Enter the reddit username and password into the fields and hit add. These are tied to your account on this site. (I'd recommend not using the same reddit user / password you normally use)</li>
        <li>Navigate to "Manage Bots"</li>
        <li>Add in all the fields. "Interval" is how often the bot will check for updates in minutes. Kindly set this number high if you know there aren't updates often for this feed to save CPU cycles. 
            These bots are also linked to your account on this site.</li>
        <li>In the list of your bots below, press "Start" to have to bot start monitoring and posting. "Stop" to stop, "Delete" to remove all data associated with that bot.</li>
        <li>?????</li>
        <li>Profit!</li>
    </ol>
</asp:Content>
