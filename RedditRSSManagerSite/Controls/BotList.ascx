<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BotList.ascx.cs" Inherits="RedditRSSManagerSite.Controls.BotList" %>


<asp:GridView runat="server" ID="gvBots" AutoGenerateColumns="false" OnRowCommand="gvBots_RowCommand" ItemType="RedditRSS.Data.RedditRSSBotData" SelectMethod="gvBots_GetData">
    <Columns>
        <asp:BoundField DataField="ID" HeaderText="ID" />
        <asp:BoundField DataField="RedditUser.Username" HeaderText="Username" />
        <asp:TemplateField HeaderText="FeedUrl">
            <ItemTemplate>
                <asp:HyperLink runat="server" NavigateUrl='<%# Item.FeedUrl %>' Text='<%# Item.FeedUrl %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Subreddit">
            <ItemTemplate>
                <asp:HyperLink runat="server" NavigateUrl='<%# "http://reddit.com/r/" + Item.Subreddit %>' Text='<%# Item.Subreddit %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="Interval" HeaderText="Interval" />
        <asp:BoundField DataField="RedditRSSBotStatusType.Description" HeaderText="Current Status" />
        <asp:TemplateField HeaderText="Actions">
            <ItemTemplate>
                <asp:Button ID="Button1" runat="server" Text="Start" CommandArgument='<%# Eval("ID") %>' CommandName="StartBot" />
                <asp:Button ID="Button2" runat="server" Text="Stop" CommandArgument='<%# Eval("ID") %>' CommandName="StopBot" />
                <asp:Button ID="Button3" runat="server" Text="Delete" CommandArgument='<%# Eval("ID") %>' CommandName="DeleteBot" />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
