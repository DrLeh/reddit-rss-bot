<%@ Page Title="Currently Running Bots" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CurrentlyRunningBots.aspx.cs" Inherits="RedditRSSManagerSite.Admin.CurrentlyRunningBots" %>

<%@ Register Src="~/Controls/BotList.ascx" TagPrefix="uc" TagName="BotList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Currently Running Bots</h2>
    <uc:BotList runat="server" id="ucBotList" DisplayMode="CurrentlyRunning" />
</asp:Content>

