<%@ Page Title="Quiz Result" Language="C#" MasterPageFile="~/Learner.Master" AutoEventWireup="true" CodeBehind="TopicQuizResult.aspx.cs" Inherits="CyberLearner.Learner.TopicQuizResult" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="page-title">Quiz Result</h1>

    <div class="card text-center">
        <h2><asp:Label ID="lblTopic" runat="server" /></h2>
        <p class="result-score"><asp:Label ID="lblScore" runat="server" />%</p>
        <p><asp:Label ID="lblStatus" runat="server" /></p>
        <p>Pass mark: <asp:Label ID="lblPassMark" runat="server" />% &nbsp;|&nbsp; Date: <asp:Label ID="lblDate" runat="server" /></p>
        <p><asp:Label ID="lblInfo" runat="server" /></p>

        <div class="button-row">
            <asp:HyperLink ID="lnkNext" runat="server" CssClass="btn btn-success" Visible="false" />
            <asp:HyperLink ID="lnkRetry" runat="server" CssClass="btn" Text="Try Quiz Again" Visible="false" />
            <asp:HyperLink ID="lnkMaterial" runat="server" CssClass="btn btn-secondary" Text="Review Material" />
            <a class="btn btn-secondary" href="Topics.aspx">All Topics</a>
        </div>
    </div>
</asp:Content>
