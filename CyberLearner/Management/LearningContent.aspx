<%@ Page Title="Learning Content" Language="C#" MasterPageFile="~/Management.Master" AutoEventWireup="true" CodeBehind="LearningContent.aspx.cs" Inherits="CyberLearner.Management.LearningContent" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="page-title">Learning Content</h1>
    <asp:Label ID="lblMessage" runat="server" Visible="false" />

    <div class="table-wrap">
        <asp:GridView ID="gvTopics" runat="server" CssClass="table" AutoGenerateColumns="false" GridLines="None"
            EmptyDataText="No topics have been added yet." OnRowCommand="gvTopics_RowCommand">
            <Columns>
                <asp:BoundField DataField="TopicOrder" HeaderText="Order" />
                <asp:BoundField DataField="TopicTitle" HeaderText="Topic Title" />
                <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>
                        <span class='badge badge-<%# Eval("Status").ToString().ToLower() %>'><%# Eval("Status") %></span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Actions">
                    <ItemTemplate>
                        <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btn btn-small" CommandName="EditTopic" CommandArgument='<%# Eval("TopicID") %>' CausesValidation="false" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>

    <div class="card" style="margin-top: 20px;">
        <h2><asp:Label ID="lblFormTitle" runat="server" Text="Add New Topic" /></h2>
        <asp:HiddenField ID="hfTopicID" runat="server" Value="0" />

        <div class="form-group">
            <label for="<%= txtTitle.ClientID %>">Topic Title</label>
            <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control" MaxLength="150" />
            <asp:RequiredFieldValidator ID="rfvTitle" runat="server" ControlToValidate="txtTitle" ErrorMessage="Topic title is required." CssClass="error" Display="Dynamic" />
        </div>

        <div class="form-group">
            <label for="<%= txtOrder.ClientID %>">Topic Order</label>
            <asp:TextBox ID="txtOrder" runat="server" CssClass="form-control" MaxLength="3" />
            <asp:RequiredFieldValidator ID="rfvOrder" runat="server" ControlToValidate="txtOrder" ErrorMessage="Topic order is required." CssClass="error" Display="Dynamic" />
            <asp:RangeValidator ID="rvOrder" runat="server" ControlToValidate="txtOrder" Type="Integer" MinimumValue="1" MaximumValue="100" ErrorMessage="Topic order must be a number from 1 to 100." CssClass="error" Display="Dynamic" />
        </div>

        <div class="form-group">
            <label for="<%= ddlStatus.ClientID %>">Status</label>
            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                <asp:ListItem Text="Active" Value="Active" />
                <asp:ListItem Text="Inactive" Value="Inactive" />
            </asp:DropDownList>
        </div>

        <div class="form-group">
            <label for="<%= txtContent.ClientID %>">Topic Content (HTML is allowed, for example headings, lists and images)</label>
            <asp:TextBox ID="txtContent" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="14" ValidateRequestMode="Disabled" />
            <asp:RequiredFieldValidator ID="rfvContent" runat="server" ControlToValidate="txtContent" ErrorMessage="Topic content is required." CssClass="error" Display="Dynamic" />
        </div>

        <asp:Button ID="btnSave" runat="server" Text="Save Topic" CssClass="btn" OnClick="btnSave_Click" />
        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-secondary" OnClick="btnClear_Click" CausesValidation="false" />
    </div>
</asp:Content>
