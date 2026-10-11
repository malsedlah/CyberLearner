<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/Learner.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="CyberLearner.Learner.Dashboard" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="page-title">Welcome, <asp:Label ID="lblName" runat="server" /></h1>

    <section class="card-grid">
        <article class="card">
            <h3>Course Progress</h3>
            <p class="stat"><asp:Label ID="lblTopics" runat="server" /></p>
            <p>topics completed</p>
            <div class="progress">
                <div class="progress-bar" style="width: <%= ProgressPercent %>%;"></div>
            </div>
            <a class="btn btn-small" href="Topics.aspx">Go to Course</a>
        </article>

        <article class="card">
            <h3>Certification Exam</h3>
            <p><asp:Label ID="lblExamStatus" runat="server" /></p>
            <p><asp:Label ID="lblExamInfo" runat="server" /></p>
            <a class="btn btn-small" href="ExamAccess.aspx">Go to Exam</a>
        </article>

        <article class="card">
            <h3>Certificate</h3>
            <p><asp:Label ID="lblCertificate" runat="server" /></p>
            <a class="btn btn-small" href="Certificate.aspx">View Certificate</a>
        </article>
    </section>

    <section class="card-grid">
        <article class="card">
            <h3>Quiz Attempts</h3>
            <p class="stat"><asp:Label ID="lblQuizAttempts" runat="server" /></p>
            <a class="btn btn-small" href="Results.aspx">View Results</a>
        </article>

        <article class="card">
            <h3>Direct Exam Request</h3>
            <p><asp:Label ID="lblDirect" runat="server" /></p>
            <a class="btn btn-small" href="Requests.aspx">View Requests</a>
        </article>

        <article class="card">
            <h3>Retake Request</h3>
            <p><asp:Label ID="lblRetake" runat="server" /></p>
            <a class="btn btn-small" href="Requests.aspx">View Requests</a>
        </article>
    </section>
</asp:Content>
