<%@ Page Title="Home" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CyberLearner._Default" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .steps {
            padding-left: 20px;
        }

        .steps li {
            margin-bottom: 6px;
        }

        .home-image {
            display: block;
            margin: 0 auto;
            max-width: 420px;
            width: 100%;
        }
    </style>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <section class="hero">
        <h1>Learn Cybersecurity Fundamentals</h1>
        <p>CyberLearner is a free online learning platform where you can study the basics of cybersecurity, test yourself with quizzes and earn a certificate.</p>
        <a class="btn btn-success" href="Register.aspx">Create an Account</a>
        <a class="btn" href="Login.aspx" style="margin-left: 8px;">Login</a>
    </section>

    <section class="card-grid">
        <article class="card">
            <h3>Learn</h3>
            <p>Read short and simple learning topics with pictures and video that explain how attacks work and how to stay safe.</p>
        </article>
        <article class="card">
            <h3>Practise</h3>
            <p>Take a quiz at the end of every topic. Pass the quiz to unlock the next topic.</p>
        </article>
        <article class="card">
            <h3>Get Certified</h3>
            <p>Pass the final certification exam and receive the Cybersecurity Fundamentals Certificate.</p>
        </article>
    </section>

    <section class="card">
        <h2>Course Topics</h2>
        <p>The Cybersecurity Fundamentals course currently has the following topics:</p>
        <asp:Repeater ID="rptTopics" runat="server">
            <HeaderTemplate>
                <ol>
            </HeaderTemplate>
            <ItemTemplate>
                <li><%#: Eval("TopicTitle") %></li>
            </ItemTemplate>
            <FooterTemplate>
                </ol>
            </FooterTemplate>
        </asp:Repeater>
        <asp:Label ID="lblNoTopics" runat="server" Text="Topics will be available soon." Visible="false" />
    </section>

    <section class="card">
        <h2>How It Works</h2>
        <img class="home-image" src="Images/Learning/cia-triad.svg" alt="Confidentiality, Integrity and Availability" />
        <ol class="steps">
            <li>Register for a free learner account.</li>
            <li>Study each topic and pass the topic quiz.</li>
            <li>Take the final certification exam.</li>
            <li>View and print your certificate.</li>
        </ol>
    </section>
</asp:Content>
