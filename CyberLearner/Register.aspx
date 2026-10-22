<%@ Page Title="Register" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="CyberLearner.Register" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="card form-box">
        <h2>Create a Learner Account</h2>
        <asp:Label ID="lblMessage" runat="server" Visible="false" />

        <asp:Panel ID="pnlForm" runat="server">
            <div class="form-group">
                <label for="<%= txtFullName.ClientID %>">Full Name</label>
                <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" MaxLength="100" />
                <asp:RequiredFieldValidator ID="rfvFullName" runat="server" ControlToValidate="txtFullName" ErrorMessage="Full name is required." CssClass="error" Display="Dynamic" />
            </div>

            <div class="form-group">
                <label for="<%= txtEmail.ClientID %>">Email</label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" MaxLength="150" TextMode="Email" />
                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Email is required." CssClass="error" Display="Dynamic" />
                <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Please enter a valid email address." ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$" CssClass="error" Display="Dynamic" />
            </div>

            <div class="form-group">
                <label for="<%= txtUsername.ClientID %>">Username</label>
                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" MaxLength="20" />
                <asp:RequiredFieldValidator ID="rfvUsername" runat="server" ControlToValidate="txtUsername" ErrorMessage="Username is required." CssClass="error" Display="Dynamic" />
                <asp:RegularExpressionValidator ID="revUsername" runat="server" ControlToValidate="txtUsername" ErrorMessage="Username must be 4 to 20 letters, numbers or underscores." ValidationExpression="^[A-Za-z0-9_]{4,20}$" CssClass="error" Display="Dynamic" />
            </div>

            <div class="form-group">
                <label for="<%= txtPassword.ClientID %>">Password</label>
                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" MaxLength="50" />
                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword" ErrorMessage="Password is required." CssClass="error" Display="Dynamic" />
                <asp:RegularExpressionValidator ID="revPassword" runat="server" ControlToValidate="txtPassword" ErrorMessage="Password must be at least 8 characters with a letter and a number." ValidationExpression="^(?=.*[A-Za-z])(?=.*\d).{8,}$" CssClass="error" Display="Dynamic" />
            </div>

            <div class="form-group">
                <label for="<%= txtConfirm.ClientID %>">Confirm Password</label>
                <asp:TextBox ID="txtConfirm" runat="server" CssClass="form-control" TextMode="Password" MaxLength="50" />
                <asp:RequiredFieldValidator ID="rfvConfirm" runat="server" ControlToValidate="txtConfirm" ErrorMessage="Please confirm your password." CssClass="error" Display="Dynamic" />
                <asp:CompareValidator ID="cvConfirm" runat="server" ControlToValidate="txtConfirm" ControlToCompare="txtPassword" ErrorMessage="Passwords do not match." CssClass="error" Display="Dynamic" />
            </div>

            <asp:Button ID="btnRegister" runat="server" Text="Register" CssClass="btn" OnClick="btnRegister_Click" />
            <p>Already have an account? <a href="Login.aspx">Login here</a></p>
        </asp:Panel>

        <asp:Panel ID="pnlOtp" runat="server" Visible="false">
            <p>We sent a 6-digit verification code to <strong><asp:Label ID="lblOtpEmail" runat="server" /></strong>. Enter it below to finish creating your account.</p>

            <div class="form-group">
                <label for="<%= txtOtp.ClientID %>">Verification Code</label>
                <asp:TextBox ID="txtOtp" runat="server" CssClass="form-control" MaxLength="6" />
                <asp:RequiredFieldValidator ID="rfvOtp" runat="server" ControlToValidate="txtOtp" ErrorMessage="Please enter the code." CssClass="error" Display="Dynamic" ValidationGroup="Otp" />
                <asp:RegularExpressionValidator ID="revOtp" runat="server" ControlToValidate="txtOtp" ErrorMessage="The code must be 6 digits." ValidationExpression="^\d{6}$" CssClass="error" Display="Dynamic" ValidationGroup="Otp" />
            </div>

            <div class="button-row">
                <asp:Button ID="btnVerify" runat="server" Text="Verify and Create Account" CssClass="btn btn-success" OnClick="btnVerify_Click" ValidationGroup="Otp" />
                <asp:Button ID="btnResend" runat="server" Text="Resend Code" CssClass="btn btn-secondary" OnClick="btnResend_Click" CausesValidation="false" />
                <asp:Button ID="btnBack" runat="server" Text="Change Details" CssClass="btn btn-secondary" OnClick="btnBack_Click" CausesValidation="false" />
            </div>
        </asp:Panel>
    </div>
</asp:Content>