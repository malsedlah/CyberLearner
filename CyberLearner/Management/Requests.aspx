<%@ Page Title="Requests" Language="C#" MasterPageFile="~/Management.Master" AutoEventWireup="true" CodeBehind="Requests.aspx.cs" Inherits="CyberLearner.Management.Requests" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="page-title">Learner Requests</h1>
    <asp:Label ID="lblMessage" runat="server" Visible="false" />

    <div class="search-row">
        <label for="<%= ddlStatus.ClientID %>">Show:</label>
        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
            <asp:ListItem Text="Pending" Value="Pending" />
            <asp:ListItem Text="Approved" Value="Approved" />
            <asp:ListItem Text="Rejected" Value="Rejected" />
            <asp:ListItem Text="All requests" Value="All" />
        </asp:DropDownList>
    </div>

    <div class="card">
        <h2>Direct Exam Access Requests</h2>
        <div class="table-wrap">
            <asp:GridView ID="gvDirect" runat="server" CssClass="table" AutoGenerateColumns="false" GridLines="None"
                EmptyDataText="No direct exam requests found." OnRowCommand="gvDirect_RowCommand">
                <Columns>
                    <asp:BoundField DataField="FullName" HeaderText="Learner" />
                    <asp:BoundField DataField="Username" HeaderText="Username" />
                    <asp:BoundField DataField="RequestDate" HeaderText="Requested" DataFormatString="{0:dd MMM yyyy HH:mm}" />
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <span class='badge badge-<%# Eval("Status").ToString().ToLower() %>'><%# Eval("Status") %></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ReviewerName" HeaderText="Reviewed By" NullDisplayText="-" />
                    <asp:BoundField DataField="ReviewedDate" HeaderText="Reviewed On" DataFormatString="{0:dd MMM yyyy HH:mm}" NullDisplayText="-" />
                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <asp:Button ID="btnApprove" runat="server" Text="Approve" CssClass="btn btn-small btn-success" CommandName="ApproveRequest" CommandArgument='<%# Eval("RequestID") %>' Visible='<%# Eval("Status").ToString() == "Pending" %>' />
                            <asp:Button ID="btnReject" runat="server" Text="Reject" CssClass="btn btn-small btn-danger" CommandName="RejectRequest" CommandArgument='<%# Eval("RequestID") %>' Visible='<%# Eval("Status").ToString() == "Pending" %>' OnClientClick="return confirm('Reject this request?');" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <div class="card">
        <h2>Exam Retake Requests</h2>
        <div class="table-wrap">
            <asp:GridView ID="gvRetake" runat="server" CssClass="table" AutoGenerateColumns="false" GridLines="None"
                EmptyDataText="No retake requests found." OnRowCommand="gvRetake_RowCommand">
                <Columns>
                    <asp:BoundField DataField="FullName" HeaderText="Learner" />
                    <asp:BoundField DataField="Username" HeaderText="Username" />
                    <asp:BoundField DataField="AttemptNumber" HeaderText="Failed Attempt" />
                    <asp:BoundField DataField="Score" HeaderText="Score (%)" DataFormatString="{0:0.##}" />
                    <asp:BoundField DataField="AttemptDate" HeaderText="Attempt Date" DataFormatString="{0:dd MMM yyyy}" />
                    <asp:BoundField DataField="RequestDate" HeaderText="Requested" DataFormatString="{0:dd MMM yyyy HH:mm}" />
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <span class='badge badge-<%# Eval("Status").ToString().ToLower() %>'><%# Eval("Status") %></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ReviewerName" HeaderText="Reviewed By" NullDisplayText="-" />
                    <asp:BoundField DataField="ReviewedDate" HeaderText="Reviewed On" DataFormatString="{0:dd MMM yyyy HH:mm}" NullDisplayText="-" />
                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <asp:Button ID="btnApprove" runat="server" Text="Approve" CssClass="btn btn-small btn-success" CommandName="ApproveRequest" CommandArgument='<%# Eval("RetakeRequestID") %>' Visible='<%# Eval("Status").ToString() == "Pending" %>' />
                            <asp:Button ID="btnReject" runat="server" Text="Reject" CssClass="btn btn-small btn-danger" CommandName="RejectRequest" CommandArgument='<%# Eval("RetakeRequestID") %>' Visible='<%# Eval("Status").ToString() == "Pending" %>' OnClientClick="return confirm('Reject this request?');" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
