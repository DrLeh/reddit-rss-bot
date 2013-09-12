<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="RedditRSSManagerSite.CustomAuth.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Login:</h2>
    <asp:Label runat="server" ID="lblMessage" />
    <table>
        <tr>
            <td>Username</td>
            <td>
                <asp:TextBox runat="server" ID="tbUsername" /></td>
        </tr>
        <tr>
            <td>Password</td>
            <td>
                <asp:TextBox runat="server" ID="tbPassword" TextMode="Password" MaxLength="30" /></td>
        </tr>
    </table>
    <table>
        <tr>
            <td>
                <asp:Button runat="server" ID="btnLogin" OnClick="btnLogin_Click" Text="Login" /></td>
        </tr>
    </table>
</asp:Content>
