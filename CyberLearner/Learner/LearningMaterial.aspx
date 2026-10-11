<%@ Page Title="Learning Material" Language="C#" MasterPageFile="~/Learner.Master" AutoEventWireup="true" CodeBehind="LearningMaterial.aspx.cs" Inherits="CyberLearner.Learner.LearningMaterial" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <p><a href="Topics.aspx">&laquo; Back to all topics</a></p>
    <h1 class="page-title"><asp:Label ID="lblTitle" runat="server" /></h1>

    <article class="card topic-content">
        <asp:Literal ID="litContent" runat="server" />
    </article>

    <div class="card">
        <h3>Ready to test yourself?</h3>
        <asp:Label ID="lblStatus" runat="server" />
        <div class="button-row">
            <asp:HyperLink ID="lnkQuiz" runat="server" CssClass="btn btn-success" Text="Take Topic Quiz" />
            <asp:HyperLink ID="lnkNext" runat="server" CssClass="btn" Text="Next Topic" Visible="false" />
        </div>
    </div>
</asp:Content>
