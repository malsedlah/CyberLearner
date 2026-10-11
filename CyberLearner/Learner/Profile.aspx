<%@ Page Title="Profile" Language="C#" MasterPageFile="~/Learner.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="CyberLearner.Learner.Profile" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="page-title">My Profile</h1>
    <asp:Label ID="lblMessage" runat="server" Visible="false" />

    <div class="card">
        <h2>Account Details</h2>
        <div class="form-group">
            <label>Username</label>
            <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" ReadOnly="true" />
        </div>
        <div class="form-group">
            <label>Member Since</label>
            <asp:TextBox ID="txtCreatedAt" runat="server" CssClass="form-control" ReadOnly="true" />
        </div>
        <div class="form-group">
            <label for="<%= txtFullName.ClientID %>">Full Name</label>
            <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" MaxLength="100" />
            <asp:RequiredFieldValidator ID="rfvFullName" runat="server" ControlToValidate="txtFullName" ErrorMessage="Full name is required." CssClass="error" Display="Dynamic" ValidationGroup="Profile" />
        </div>
        <div class="form-group">
            <label for="<%= txtEmail.ClientID %>">Email</label>
            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" MaxLength="150" />
            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Email is required." CssClass="error" Display="Dynamic" ValidationGroup="Profile" />
            <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Please enter a valid email address." ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$" CssClass="error" Display="Dynamic" ValidationGroup="Profile" />
        </div>
        <asp:Button ID="btnSave" runat="server" Text="Save Changes" CssClass="btn" OnClick="btnSave_Click" ValidationGroup="Profile" />
    </div>

    <div class="card">
        <h2>Change Password</h2>
        <div class="form-group">
            <label for="<%= txtCurrent.ClientID %>">Current Password</label>
            <asp:TextBox ID="txtCurrent" runat="server" CssClass="form-control" TextMode="Password" MaxLength="50" />
            <asp:RequiredFieldValidator ID="rfvCurrent" runat="server" ControlToValidate="txtCurrent" ErrorMessage="Current password is required." CssClass="error" Display="Dynamic" ValidationGroup="Password" />
        </div>
        <div class="form-group">
            <label for="<%= txtNew.ClientID %>">New Password</label>
            <asp:TextBox ID="txtNew" runat="server" CssClass="form-control" TextMode="Password" MaxLength="50" />
            <asp:RequiredFieldValidator ID="rfvNew" runat="server" ControlToValidate="txtNew" ErrorMessage="New password is required." CssClass="error" Display="Dynamic" ValidationGroup="Password" />
            <asp:RegularExpressionValidator ID="revNew" runat="server" ControlToValidate="txtNew" ErrorMessage="Password must be at least 8 characters with a letter and a number." ValidationExpression="^(?=.*[A-Za-z])(?=.*\d).{8,}$" CssClass="error" Display="Dynamic" ValidationGroup="Password" />
        </div>
        <div class="form-group">
            <label for="<%= txtConfirm.ClientID %>">Confirm New Password</label>
            <asp:TextBox ID="txtConfirm" runat="server" CssClass="form-control" TextMode="Password" MaxLength="50" />
            <asp:RequiredFieldValidator ID="rfvConfirm" runat="server" ControlToValidate="txtConfirm" ErrorMessage="Please confirm your new password." CssClass="error" Display="Dynamic" ValidationGroup="Password" />
            <asp:CompareValidator ID="cvConfirm" runat="server" ControlToValidate="txtConfirm" ControlToCompare="txtNew" ErrorMessage="Passwords do not match." CssClass="error" Display="Dynamic" ValidationGroup="Password" />
        </div>
        <asp:Button ID="btnPassword" runat="server" Text="Change Password" CssClass="btn" OnClick="btnPassword_Click" ValidationGroup="Password" />
    </div>
</asp:Content>
