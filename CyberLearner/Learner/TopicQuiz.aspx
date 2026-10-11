<%@ Page Title="Topic Quiz" Language="C#" MasterPageFile="~/Learner.Master" AutoEventWireup="true" CodeBehind="TopicQuiz.aspx.cs" Inherits="CyberLearner.Learner.TopicQuiz" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="page-title">Quiz: <asp:Label ID="lblTopic" runat="server" /></h1>
    <asp:Label ID="lblMessage" runat="server" Visible="false" />

    <asp:Panel ID="pnlQuiz" runat="server" CssClass="card question-box">
        <p><asp:Label ID="lblProgress" runat="server" /></p>
        <p class="question-text"><asp:Label ID="lblQuestion" runat="server" /></p>
        <asp:RadioButtonList ID="rblOptions" runat="server" CssClass="options" />

        <div class="button-row">
            <asp:Button ID="btnPrevious" runat="server" Text="Previous" CssClass="btn btn-secondary" OnClick="btnPrevious_Click" />
            <asp:Button ID="btnNext" runat="server" Text="Next" CssClass="btn" OnClick="btnNext_Click" OnClientClick="return checkAnswerSelected();" />
            <asp:Button ID="btnSubmit" runat="server" Text="Submit Quiz" CssClass="btn btn-success" OnClick="btnSubmit_Click" OnClientClick="return checkAnswerSelected() && confirmSubmit();" />
        </div>
    </asp:Panel>

    <p><asp:HyperLink ID="lnkBack" runat="server" Text="Back to learning material" /></p>
</asp:Content>
