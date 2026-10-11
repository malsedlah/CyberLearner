<%@ Page Title="Exam Guidelines" Language="C#" MasterPageFile="~/Learner.Master" AutoEventWireup="true" CodeBehind="ExamGuidelines.aspx.cs" Inherits="CyberLearner.Learner.ExamGuidelines" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="page-title">Exam Guidelines</h1>
    <asp:Label ID="lblMessage" runat="server" Visible="false" />

    <div class="card">
        <p>Please read the guidelines carefully before you start the exam.</p>
        <ol>
            <li>The exam has <asp:Label ID="lblQuestionCount" runat="server" /> multiple choice questions.</li>
            <li>You need at least <asp:Label ID="lblPassMark" runat="server" />% to pass.</li>
            <li>Questions are shown one at a time. You can go back to a previous question before submitting.</li>
            <li>Every question must be answered before you can submit.</li>
            <li>Your score is calculated automatically when you submit.</li>
            <li>If you fail, you must wait <asp:Label ID="lblWaitDays" runat="server" /> days and then request a retake from management.</li>
            <li>Do not close the browser or log out during the exam.</li>
            <li>This is an individual exam. Do not use help from other people.</li>
        </ol>

        <p>
            <asp:CheckBox ID="chkAgree" runat="server" Text=" I have read and understood the exam guidelines." />
        </p>
        <asp:Button ID="btnStart" runat="server" Text="Start Exam" CssClass="btn btn-success" OnClick="btnStart_Click" />
        <a class="btn btn-secondary" href="ExamAccess.aspx">Back</a>
    </div>

    <script>
        var agreeBox = document.getElementById("<%= chkAgree.ClientID %>");
        var startButton = document.getElementById("<%= btnStart.ClientID %>");
        startButton.disabled = !agreeBox.checked;
        agreeBox.onclick = function () {
            toggleStartButton(agreeBox, startButton.id);
        };
    </script>
</asp:Content>
