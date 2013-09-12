<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BotList.ascx.cs" Inherits="RedditRSSManagerSite.Controls.BotList" %>


<asp:GridView runat="server" ID="gvBots" AutoGenerateColumns="false" OnRowCommand="gvBots_RowCommand" ItemType="RedditRSS.RedditRSSBot" SelectMethod="gvBots_GetData">
    <Columns>
        <asp:BoundField DataField="ID" HeaderText="ID" />
        <asp:BoundField DataField="RedditRSSBotData.RedditUser.Username" HeaderText="Bot" />
        <asp:TemplateField HeaderText="FeedUrl">
            <ItemTemplate>
                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Item.RedditRSSBotData.FeedUrl %>' Text='<%# Item.RedditRSSBotData.FeedUrl %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Subreddit">
            <ItemTemplate>
                <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl='<%# "http://reddit.com/r/" + Item.RedditRSSBotData.Subreddit %>' Text='<%# Item.RedditRSSBotData.Subreddit %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="Interval" HeaderText="Interval" />
        <asp:BoundField DataField="CurrentStatus" HeaderText="Current Status" />
        <asp:TemplateField HeaderText="Actions">
            <ItemTemplate>
                <asp:Button ID="Button1" runat="server" Text="Start" CommandArgument='<%# Eval("ID") %>' CommandName="StartBot" />
                <asp:Button ID="Button2" runat="server" Text="Stop" CommandArgument='<%# Eval("ID") %>' CommandName="StopBot" />
                <asp:Button ID="Button3" runat="server" Text="Delete" CommandArgument='<%# Eval("RedditRSSBotData.ID") %>' CommandName="DeleteBot" />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
