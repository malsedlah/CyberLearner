<%@ Page Title="Certificates" Language="C#" MasterPageFile="~/Management.Master" AutoEventWireup="true" CodeBehind="Certificates.aspx.cs" Inherits="CyberLearner.Management.Certificates" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="page-title">Certificates</h1>
    <p>Certificates are created automatically when a learner passes the certification exam. Total issued: <strong><asp:Label ID="lblTotal" runat="server" /></strong></p>

    <div class="search-row">
        <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Search name, email or certificate number" MaxLength="50" />
        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn" OnClick="btnSearch_Click" />
        <asp:Button ID="btnShowAll" runat="server" Text="Show All" CssClass="btn btn-secondary" OnClick="btnShowAll_Click" />
    </div>

    <div class="table-wrap">
        <asp:GridView ID="gvCertificates" runat="server" CssClass="table" AutoGenerateColumns="false" GridLines="None"
            EmptyDataText="No certificates found.">
            <Columns>
                <asp:BoundField DataField="CertificateNumber" HeaderText="Certificate Number" />
                <asp:BoundField DataField="FullName" HeaderText="Learner" />
                <asp:BoundField DataField="Email" HeaderText="Email" />
                <asp:BoundField DataField="AttemptNumber" HeaderText="Exam Attempt" />
                <asp:BoundField DataField="Score" HeaderText="Score (%)" DataFormatString="{0:0.##}" />
                <asp:BoundField DataField="IssueDate" HeaderText="Issue Date" DataFormatString="{0:dd MMM yyyy}" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
