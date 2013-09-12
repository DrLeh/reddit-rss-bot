<%@ Page Title="Manage Reddit Users" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageRedditUsers.aspx.cs" Inherits="RedditRSSManagerSite.ManageRedditUsers" %>


<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Add / edit reddit users</h2>
    <table>
        <tr>
            <td>User Name</td>
            <td><asp:TextBox runat="server" ID="tbUsername" /></td>
        </tr>
        <tr>
            <td>Password</td>
            <td><asp:TextBox runat="server" TextMode="Password" ID="tbPassword" /></td>
        </tr>
    </table>
    <table>
        <asp:Button runat="server" ID="btnSave" OnClick="btnSave_Click" Text="Save" />
    </table>
    <asp:GridView runat="server" ID="gvUsers" ItemType="RedditRSS.Data.RedditUser" OnRowCommand="gvUsers_RowCommand"
        SelectMethod="gvUsers_GetData" DataKeyNames="UserName" AutoGenerateColumns="false">
        <Columns>
            <asp:BoundField DataField="Username" />
            <asp:TemplateField HeaderText="Actions">
                <ItemTemplate>
                <asp:Button ID="Button3" runat="server" Text="Delete" CommandArgument='<%# Eval("ID") %>' CommandName="DeleteUser" />
            </ItemTemplate>
        </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>
