<%@ Page Title="Requests" Language="C#" MasterPageFile="~/Learner.Master" AutoEventWireup="true" CodeBehind="Requests.aspx.cs" Inherits="CyberLearner.Learner.Requests" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="page-title">My Requests</h1>
    <asp:Label ID="lblMessage" runat="server" Visible="false" />

    <div class="card">
        <h2>Direct Exam Access</h2>
        <p>If you already know the course material, you can ask management to let you take the certification exam without finishing all the topics.</p>
        <p><strong>Status:</strong> <asp:Label ID="lblDirectStatus" runat="server" /></p>
        <asp:Button ID="btnDirect" runat="server" Text="Request Direct Exam Access" CssClass="btn" OnClick="btnDirect_Click" Visible="false" />

        <div class="table-wrap" style="margin-top: 15px;">
            <asp:GridView ID="gvDirect" runat="server" CssClass="table" AutoGenerateColumns="false" GridLines="None"
                EmptyDataText="You have not made any direct exam requests.">
                <Columns>
                    <asp:BoundField DataField="RequestDate" HeaderText="Requested" DataFormatString="{0:dd MMM yyyy HH:mm}" />
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <span class='badge badge-<%# Eval("Status").ToString().ToLower() %>'><%# Eval("Status") %></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ReviewerName" HeaderText="Reviewed By" NullDisplayText="-" />
                    <asp:BoundField DataField="ReviewedDate" HeaderText="Reviewed On" DataFormatString="{0:dd MMM yyyy HH:mm}" NullDisplayText="-" />
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <div class="card">
        <h2>Exam Retake</h2>
        <p>If you fail the certification exam, you must wait <asp:Label ID="lblWaitDays" runat="server" /> days and then request a retake. Each approved request gives one new attempt.</p>
        <p><strong>Status:</strong> <asp:Label ID="lblRetakeStatus" runat="server" /></p>
        <asp:Button ID="btnRetake" runat="server" Text="Request Exam Retake" CssClass="btn" OnClick="btnRetake_Click" Visible="false" />

        <div class="table-wrap" style="margin-top: 15px;">
            <asp:GridView ID="gvRetake" runat="server" CssClass="table" AutoGenerateColumns="false" GridLines="None"
                EmptyDataText="You have not made any retake requests.">
                <Columns>
                    <asp:BoundField DataField="AttemptNumber" HeaderText="Failed Attempt" />
                    <asp:BoundField DataField="Score" HeaderText="Score (%)" DataFormatString="{0:0.##}" />
                    <asp:BoundField DataField="RequestDate" HeaderText="Requested" DataFormatString="{0:dd MMM yyyy HH:mm}" />
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <span class='badge badge-<%# Eval("Status").ToString().ToLower() %>'><%# Eval("Status") %></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ReviewerName" HeaderText="Reviewed By" NullDisplayText="-" />
                    <asp:BoundField DataField="ReviewedDate" HeaderText="Reviewed On" DataFormatString="{0:dd MMM yyyy HH:mm}" NullDisplayText="-" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
