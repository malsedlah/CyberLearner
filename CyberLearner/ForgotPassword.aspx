<%@ Page Title="Forgot Password" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="CyberLearner.ForgotPassword" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="card form-box">
        <h2>Reset Password</h2>
        <asp:Label ID="lblMessage" runat="server" Visible="false" />

        <asp:Panel ID="pnlEmail" runat="server">
            <p>Step 1 of 3: Enter the email address of your account.</p>
            <div class="form-group">
                <label for="<%= txtEmail.ClientID %>">Email</label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" MaxLength="150" TextMode="Email" />
                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Email is required." CssClass="error" Display="Dynamic" ValidationGroup="Email" />
                <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Please enter a valid email address." ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$" CssClass="error" Display="Dynamic" ValidationGroup="Email" />
            </div>
            <asp:Button ID="btnSendCode" runat="server" Text="Send Code" CssClass="btn" OnClick="btnSendCode_Click" ValidationGroup="Email" />
        </asp:Panel>

        <asp:Panel ID="pnlCode" runat="server" Visible="false">
            <p>Step 2 of 3: Enter the verification code.</p>
            <p class="alert alert-info">Demo only: your verification code is <strong><asp:Label ID="lblCode" runat="server" /></strong>. In a real system this code would be sent by email.</p>
            <div class="form-group">
                <label for="<%= txtCode.ClientID %>">Verification Code</label>
                <asp:TextBox ID="txtCode" runat="server" CssClass="form-control" MaxLength="6" />
                <asp:RequiredFieldValidator ID="rfvCode" runat="server" ControlToValidate="txtCode" ErrorMessage="Code is required." CssClass="error" Display="Dynamic" ValidationGroup="Code" />
            </div>
            <asp:Button ID="btnVerify" runat="server" Text="Verify Code" CssClass="btn" OnClick="btnVerify_Click" ValidationGroup="Code" />
        </asp:Panel>

        <asp:Panel ID="pnlPassword" runat="server" Visible="false">
            <p>Step 3 of 3: Enter your new password.</p>
            <div class="form-group">
                <label for="<%= txtPassword.ClientID %>">New Password</label>
                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" MaxLength="50" />
                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword" ErrorMessage="Password is required." CssClass="error" Display="Dynamic" ValidationGroup="Password" />
                <asp:RegularExpressionValidator ID="revPassword" runat="server" ControlToValidate="txtPassword" ErrorMessage="Password must be at least 8 characters with a letter and a number." ValidationExpression="^(?=.*[A-Za-z])(?=.*\d).{8,}$" CssClass="error" Display="Dynamic" ValidationGroup="Password" />
            </div>
            <div class="form-group">
                <label for="<%= txtConfirm.ClientID %>">Confirm New Password</label>
                <asp:TextBox ID="txtConfirm" runat="server" CssClass="form-control" TextMode="Password" MaxLength="50" />
                <asp:RequiredFieldValidator ID="rfvConfirm" runat="server" ControlToValidate="txtConfirm" ErrorMessage="Please confirm your password." CssClass="error" Display="Dynamic" ValidationGroup="Password" />
                <asp:CompareValidator ID="cvConfirm" runat="server" ControlToValidate="txtConfirm" ControlToCompare="txtPassword" ErrorMessage="Passwords do not match." CssClass="error" Display="Dynamic" ValidationGroup="Password" />
            </div>
            <asp:Button ID="btnReset" runat="server" Text="Change Password" CssClass="btn" OnClick="btnReset_Click" ValidationGroup="Password" />
        </asp:Panel>

        <p><a href="Login.aspx">Back to login</a></p>
    </div>
</asp:Content>
