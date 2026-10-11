<%@ Page Title="Learners" Language="C#" MasterPageFile="~/Management.Master" AutoEventWireup="true" CodeBehind="Learners.aspx.cs" Inherits="CyberLearner.Management.Learners" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="page-title">Learners</h1>
    <asp:Label ID="lblMessage" runat="server" Visible="false" />

    <div class="search-row">
        <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Search name, email or username" MaxLength="50" />
        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn" OnClick="btnSearch_Click" CausesValidation="false" />
        <asp:Button ID="btnShowAll" runat="server" Text="Show All" CssClass="btn btn-secondary" OnClick="btnShowAll_Click" CausesValidation="false" />
    </div>

    <div class="table-wrap">
        <asp:GridView ID="gvLearners" runat="server" CssClass="table" AutoGenerateColumns="false" GridLines="None"
            EmptyDataText="No learners found." OnRowCommand="gvLearners_RowCommand">
            <Columns>
                <asp:BoundField DataField="UserID" HeaderText="ID" />
                <asp:BoundField DataField="FullName" HeaderText="Full Name" />
                <asp:BoundField DataField="Email" HeaderText="Email" />
                <asp:BoundField DataField="Username" HeaderText="Username" />
                <asp:BoundField DataField="CreatedAt" HeaderText="Registered" DataFormatString="{0:dd MMM yyyy}" />
                <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>
                        <span class='badge badge-<%# Eval("AccountStatus").ToString().ToLower() %>'><%# Eval("AccountStatus") %></span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Actions">
                    <ItemTemplate>
                        <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btn btn-small" CommandName="EditLearner" CommandArgument='<%# Eval("UserID") %>' CausesValidation="false" />
                        <asp:Button ID="btnStatus" runat="server" CssClass="btn btn-small btn-secondary" CommandName="ChangeStatus" CommandArgument='<%# Eval("UserID") %>' CausesValidation="false"
                            Text='<%# Eval("AccountStatus").ToString() == "Active" ? "Deactivate" : "Activate" %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>

    <asp:Panel ID="pnlEdit" runat="server" Visible="false" CssClass="card" Style="margin-top: 20px;">
        <h2>Edit Learner</h2>
        <asp:HiddenField ID="hfUserID" runat="server" />
        <div class="form-group">
            <label for="<%= txtFullName.ClientID %>">Full Name</label>
            <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" MaxLength="100" />
            <asp:RequiredFieldValidator ID="rfvFullName" runat="server" ControlToValidate="txtFullName" ErrorMessage="Full name is required." CssClass="error" Display="Dynamic" />
        </div>
        <div class="form-group">
            <label for="<%= txtEmail.ClientID %>">Email</label>
            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" MaxLength="150" />
            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Email is required." CssClass="error" Display="Dynamic" />
            <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Please enter a valid email address." ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$" CssClass="error" Display="Dynamic" />
        </div>
        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn" OnClick="btnSave_Click" />
        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-secondary" OnClick="btnCancel_Click" CausesValidation="false" />
    </asp:Panel>
</asp:Content>
