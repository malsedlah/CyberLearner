<%@ Page Title="Results" Language="C#" MasterPageFile="~/Learner.Master" AutoEventWireup="true" CodeBehind="Results.aspx.cs" Inherits="CyberLearner.Learner.Results" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="page-title">My Results</h1>

    <div class="card">
        <h2>Certification Exam Attempts</h2>
        <div class="table-wrap">
            <asp:GridView ID="gvExam" runat="server" CssClass="table" AutoGenerateColumns="false" GridLines="None"
                EmptyDataText="You have not taken the certification exam yet.">
                <Columns>
                    <asp:BoundField DataField="AttemptNumber" HeaderText="Attempt" />
                    <asp:BoundField DataField="Score" HeaderText="Score (%)" DataFormatString="{0:0.##}" />
                    <asp:TemplateField HeaderText="Result">
                        <ItemTemplate>
                            <span class='badge badge-<%# Eval("PassStatus").ToString().ToLower() %>'><%# Eval("PassStatus") %></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="AttemptDate" HeaderText="Date" DataFormatString="{0:dd MMM yyyy HH:mm}" />
                    <asp:HyperLinkField Text="View" DataNavigateUrlFields="ExamAttemptID" DataNavigateUrlFormatString="ExamResult.aspx?attemptId={0}" HeaderText="Details" />
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <div class="card">
        <h2>Topic Quiz Attempts</h2>
        <div class="table-wrap">
            <asp:GridView ID="gvQuiz" runat="server" CssClass="table" AutoGenerateColumns="false" GridLines="None"
                EmptyDataText="You have not taken any topic quizzes yet.">
                <Columns>
                    <asp:BoundField DataField="TopicTitle" HeaderText="Topic" />
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
    </div>
</asp:Content>
