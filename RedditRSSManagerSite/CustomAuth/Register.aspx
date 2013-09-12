<%@ Page Title="Register" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="RedditRSSManagerSite.CustomAuth.Register" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
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
        <tr>
            <td>Confirm Password</td>
            <td>
                <asp:TextBox runat="server" ID="tbPasswordConfirm" TextMode="Password" MaxLength="30" /></td>
        </tr>
    </table>
    <table>
        <tr>
            <td>
                <asp:Button runat="server" ID="btnRegister" OnClick="btnRegister_Click" Text="Register" /></td>
        </tr>
    </table>
</asp:Content>
