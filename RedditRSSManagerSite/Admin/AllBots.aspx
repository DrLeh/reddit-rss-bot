<%@ Page Title="All Bots" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AllBots.aspx.cs" Inherits="RedditRSSManagerSite.Admin.AllBots" %>

<%@ Register Src="~/Controls/BotList.ascx" TagPrefix="uc" TagName="BotList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <h2>All Bots</h2>
    <uc:BotList runat="server" ID="ucBotList" DisplayMode="All" />
</asp:Content>
