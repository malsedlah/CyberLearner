<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="CyberLearner.Login" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="card form-box">
        <h2>Login</h2>
        <asp:Label ID="lblMessage" runat="server" Visible="false" />

        <div class="form-group">
            <label for="<%= txtUsername.ClientID %>">Username or Email</label>
            <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" MaxLength="150" />
            <asp:RequiredFieldValidator ID="rfvUsername" runat="server" ControlToValidate="txtUsername" ErrorMessage="Username is required." CssClass="error" Display="Dynamic" />
        </div>

        <div class="form-group">
            <label for="<%= txtPassword.ClientID %>">Password</label>
            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" MaxLength="50" />
            <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword" ErrorMessage="Password is required." CssClass="error" Display="Dynamic" />
        </div>

        <div class="form-group">
            <input type="checkbox" id="chkShow" onclick="togglePassword('<%= txtPassword.ClientID %>', this)" />
            <label for="chkShow" style="display: inline; font-weight: normal;">Show password</label>
        </div>

        <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn" OnClick="btnLogin_Click" />
        <p><a href="ForgotPassword.aspx">Forgot your password?</a></p>
        <p>New here? <a href="Register.aspx">Create an account</a></p>
    </div>
</asp:Content>
