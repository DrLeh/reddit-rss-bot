<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageRSSFeeds.aspx.cs" Inherits="RedditRSSSiteManager.ManageRSSFeeds" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label runat="server" ID="lblMessages" />
    <asp:Button runat="server" Text="Refresh" />
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
                <asp:TextBox runat="server" ID="tbInterval" />
            </td>
        </tr>
        <tr>
            <td>
                Reddit User
            </td>
            <td>
                <asp:DropDownList runat="server" ID="ddlUsers" DataTextField="Username" DataValueField="Username" />
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td><asp:Button runat="server" ID="btnAddFeed" Text="Add Feed" OnClick="btnAddFeed_Click" /></td>
        </tr>
    </table>
    <asp:GridView runat="server" ID="gvBots" AutoGenerateColumns="false" OnRowCommand="gvBots_RowCommand" >
        <Columns>
            <asp:BoundField DataField="ID" HeaderText="ID" />
            <asp:BoundField DataField="Username" HeaderText="Bot" />
            <asp:BoundField DataField="FeedUrl" HeaderText="Feed Url" />
            <asp:BoundField DataField="Subreddit" HeaderText="Subreddit" />
            <asp:BoundField DataField="Interval" HeaderText="Interval" />
            <asp:BoundField DataField="CurrentStatus" HeaderText="Current Status" />
            <asp:TemplateField HeaderText="Actions">
                <ItemTemplate>
                    <asp:Button ID="Button1" runat="server" Text="Start" CommandArgument='<%# Eval("ID") %>' CommandName="StartBot" />
                    <asp:Button ID="Button2" runat="server" Text="Stop" CommandArgument='<%# Eval("ID") %>' CommandName="StopBot" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>
