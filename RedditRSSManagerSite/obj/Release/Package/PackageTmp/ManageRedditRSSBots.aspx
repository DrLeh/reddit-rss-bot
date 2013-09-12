<%@ Page Title="Manage RSS Bots" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageRedditRSSBots.aspx.cs" Inherits="RedditRSSManagerSite.ManageRedditRSSBots" %>

<%@ Register Src="~/Controls/BotList.ascx" TagPrefix="uc" TagName="BotList" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:Label runat="server" ID="lblMessages" />
            <h2>Add Bots</h2>
            <table>
                <tr>
                    <td>Feed URL
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="tbFeedUrl" />
                    </td>
                </tr>
                <tr>
                    <td>SubReddit
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="tbSubReddit" />
                    </td>
                </tr>
                <tr>
                    <td>Interval
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="tbInterval" TextMode="Number" />
                    </td>
                </tr>
                <tr>
                    <td>Reddit User
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlUsers" DataTextField="Username" DataValueField="ID"
                            ItemType="RedditRSS.Data.RedditUser" SelectMethod="ddlUsers_GetData" />
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <asp:Button runat="server" ID="btnAddFeed" Text="Add Feed" OnClick="btnAddFeed_Click" /></td>
                </tr>
            </table>
            <h2>Current Bots</h2>
            <uc:BotList runat="server" ID="ucBotList" DisplayMode="SingleUser" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
