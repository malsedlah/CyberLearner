<%@ Page Title="Course" Language="C#" MasterPageFile="~/Learner.Master" AutoEventWireup="true" CodeBehind="Topics.aspx.cs" Inherits="CyberLearner.Learner.Topics" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="page-title">Cybersecurity Fundamentals</h1>
    <p>Study each topic and pass its quiz to unlock the next topic. You need <asp:Label ID="lblPassMark" runat="server" />% to pass a topic quiz.</p>

    <p><strong>Your progress:</strong> <asp:Label ID="lblProgress" runat="server" /></p>
    <div class="progress">
        <div class="progress-bar" style="width: <%= ProgressPercent %>%;"></div>
    </div>

    <asp:Repeater ID="rptTopics" runat="server">
        <ItemTemplate>
            <div class='card topic-item <%# Eval("State").ToString() == "Locked" ? "locked" : "" %>'>
                <div>
                    <h3>Topic <%# Eval("TopicOrder") %>: <%#: Eval("TopicTitle") %></h3>
                    <span class='badge badge-<%# Eval("State").ToString().ToLower() %>'><%# Eval("State") %></span>
                </div>
                <div>
                    <asp:HyperLink ID="lnkOpen" runat="server" CssClass="btn" Text="Open Topic"
                        NavigateUrl='<%# "LearningMaterial.aspx?topicId=" + Eval("TopicID") %>'
                        Visible='<%# Eval("State").ToString() != "Locked" %>' />
                    <asp:Label ID="lblLocked" runat="server" Text="Pass the previous quiz to unlock"
                        Visible='<%# Eval("State").ToString() == "Locked" %>' />
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>

    <asp:Label ID="lblNoTopics" runat="server" CssClass="alert alert-info" Text="There are no topics available yet." Visible="false" />

    <asp:Panel ID="pnlCompleted" runat="server" Visible="false" CssClass="alert alert-success">
        You have completed all topics. <a href="ExamAccess.aspx">Go to the certification exam</a>
    </asp:Panel>
</asp:Content>
