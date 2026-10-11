<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/Management.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="CyberLearner.Management.Dashboard" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="page-title">Management Dashboard</h1>

    <section class="card-grid">
        <article class="card">
            <h3>Learners</h3>
            <p class="stat"><asp:Label ID="lblLearners" runat="server" /></p>
            <p><asp:Label ID="lblActiveLearners" runat="server" /> active</p>
            <a class="btn btn-small" href="Learners.aspx">Manage Learners</a>
        </article>

        <article class="card">
            <h3>Pending Requests</h3>
            <p class="stat"><asp:Label ID="lblPending" runat="server" /></p>
            <p>waiting for review</p>
            <a class="btn btn-small" href="Requests.aspx">Review Requests</a>
        </article>

        <article class="card">
            <h3>Certificates Issued</h3>
            <p class="stat"><asp:Label ID="lblCertificates" runat="server" /></p>
            <p>learners certified</p>
            <a class="btn btn-small" href="Certificates.aspx">View Certificates</a>
        </article>
    </section>

    <section class="card-grid">
        <article class="card">
            <h3>Active Topics</h3>
            <p class="stat"><asp:Label ID="lblTopics" runat="server" /></p>
            <a class="btn btn-small" href="LearningContent.aspx">Manage Content</a>
        </article>

        <article class="card">
            <h3>Quiz Questions</h3>
            <p class="stat"><asp:Label ID="lblQuizQuestions" runat="server" /></p>
            <a class="btn btn-small" href="TopicQuizzes.aspx">Manage Quizzes</a>
        </article>

        <article class="card">
            <h3>Exam Questions</h3>
            <p class="stat"><asp:Label ID="lblExamQuestions" runat="server" /></p>
            <a class="btn btn-small" href="CertificationExam.aspx">Manage Exam</a>
        </article>
    </section>

    <section class="card">
        <h2>Exam Summary</h2>
        <p>Total exam attempts: <strong><asp:Label ID="lblAttempts" runat="server" /></strong> &nbsp;|&nbsp; Passed: <strong><asp:Label ID="lblPassed" runat="server" /></strong> &nbsp;|&nbsp; Pass rate: <strong><asp:Label ID="lblPassRate" runat="server" /></strong></p>

        <h3>Latest Exam Attempts</h3>
        <div class="table-wrap">
            <asp:GridView ID="gvLatest" runat="server" CssClass="table" AutoGenerateColumns="false" GridLines="None"
                EmptyDataText="No exam attempts yet.">
                <Columns>
                    <asp:BoundField DataField="FullName" HeaderText="Learner" />
                    <asp:BoundField DataField="AttemptNumber" HeaderText="Attempt" />
                    <asp:BoundField DataField="Score" HeaderText="Score (%)" DataFormatString="{0:0.##}" />
                    <asp:TemplateField HeaderText="Result">
                        <ItemTemplate>
                            <span class='badge badge-<%# Eval("PassStatus").ToString().ToLower() %>'><%# Eval("PassStatus") %></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="AttemptDate" HeaderText="Date" DataFormatString="{0:dd MMM yyyy HH:mm}" />
                </Columns>
            </asp:GridView>
        </div>
        <p><a href="LearnerResults.aspx">View all learner results</a></p>
    </section>
</asp:Content>
