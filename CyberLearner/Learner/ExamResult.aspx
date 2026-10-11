<%@ Page Title="Exam Result" Language="C#" MasterPageFile="~/Learner.Master" AutoEventWireup="true" CodeBehind="ExamResult.aspx.cs" Inherits="CyberLearner.Learner.ExamResult" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="page-title">Exam Result</h1>

    <div class="card text-center">
        <h2>Cybersecurity Fundamentals Certification Exam</h2>
        <p>Attempt number <asp:Label ID="lblAttemptNumber" runat="server" /></p>
        <p class="result-score"><asp:Label ID="lblScore" runat="server" />%</p>
        <p><asp:Label ID="lblStatus" runat="server" /></p>
        <p>Pass mark: <asp:Label ID="lblPassMark" runat="server" />% &nbsp;|&nbsp; Date: <asp:Label ID="lblDate" runat="server" /></p>
        <p><asp:Label ID="lblInfo" runat="server" /></p>

        <div class="button-row">
            <asp:HyperLink ID="lnkCertificate" runat="server" CssClass="btn btn-success" Text="View Certificate" NavigateUrl="Certificate.aspx" Visible="false" />
            <asp:HyperLink ID="lnkRequests" runat="server" CssClass="btn" Text="Go to Requests" NavigateUrl="Requests.aspx" Visible="false" />
            <a class="btn btn-secondary" href="Results.aspx">All My Results</a>
        </div>
    </div>
</asp:Content>
