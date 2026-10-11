<%@ Page Title="Exam Access" Language="C#" MasterPageFile="~/Learner.Master" AutoEventWireup="true" CodeBehind="ExamAccess.aspx.cs" Inherits="CyberLearner.Learner.ExamAccess" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="page-title">Certification Exam</h1>

    <div class="card">
        <h2>Cybersecurity Fundamentals Certification Exam</h2>
        <ul>
            <li>Number of questions: <asp:Label ID="lblQuestionCount" runat="server" /></li>
            <li>Pass mark: <asp:Label ID="lblPassMark" runat="server" />%</li>
            <li>Your attempts so far: <asp:Label ID="lblAttempts" runat="server" /></li>
        </ul>
    </div>

    <div class="card">
        <h2>Your Access Status</h2>
        <asp:Label ID="lblStatus" runat="server" />
        <p><asp:Label ID="lblDetails" runat="server" /></p>

        <div class="button-row">
            <asp:HyperLink ID="lnkGuidelines" runat="server" CssClass="btn btn-success" Text="Continue to Exam Guidelines" NavigateUrl="ExamGuidelines.aspx" Visible="false" />
            <asp:HyperLink ID="lnkCourse" runat="server" CssClass="btn" Text="Continue the Course" NavigateUrl="Topics.aspx" Visible="false" />
            <asp:HyperLink ID="lnkRequests" runat="server" CssClass="btn btn-secondary" Text="Go to Requests" NavigateUrl="Requests.aspx" Visible="false" />
            <asp:HyperLink ID="lnkCertificate" runat="server" CssClass="btn btn-success" Text="View Certificate" NavigateUrl="Certificate.aspx" Visible="false" />
            <asp:HyperLink ID="lnkResults" runat="server" CssClass="btn btn-secondary" Text="View My Results" NavigateUrl="Results.aspx" Visible="false" />
        </div>
    </div>
</asp:Content>
