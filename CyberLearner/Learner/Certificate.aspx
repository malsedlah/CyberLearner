<%@ Page Title="Certificate" Language="C#" MasterPageFile="~/Learner.Master" AutoEventWireup="true" CodeBehind="Certificate.aspx.cs" Inherits="CyberLearner.Learner.Certificate" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .certificate {
            background-color: #ffffff;
            border: 10px double #12284c;
            padding: 40px 30px;
            text-align: center;
            max-width: 800px;
            margin: 0 auto 20px auto;
        }

        .certificate h2 {
            font-size: 30px;
            margin: 0 0 10px 0;
        }

        .certificate .learner-name {
            font-size: 28px;
            font-weight: bold;
            color: #1565c0;
            border-bottom: 2px solid #9aa5b1;
            display: inline-block;
            padding: 0 30px 5px 30px;
        }

        .certificate .details {
            margin-top: 25px;
            font-size: 14px;
            color: #52606d;
        }
    </style>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="page-title no-print">My Certificate</h1>

    <asp:Panel ID="pnlNoCertificate" runat="server" CssClass="card" Visible="false">
        <h2>No certificate yet</h2>
        <p>You will receive the Cybersecurity Fundamentals Certificate after you pass the certification exam.</p>
        <a class="btn" href="ExamAccess.aspx">Go to Exam</a>
    </asp:Panel>

    <asp:Panel ID="pnlCertificate" runat="server" Visible="false">
        <div class="certificate">
            <p>CyberLearner</p>
            <h2>Certificate of Completion</h2>
            <p>This is to certify that</p>
            <p class="learner-name"><asp:Label ID="lblName" runat="server" /></p>
            <p>has successfully passed the certification exam for</p>
            <h3>Cybersecurity Fundamentals</h3>
            <p>with a score of <strong><asp:Label ID="lblScore" runat="server" />%</strong></p>
            <p class="details">
                Certificate Number: <asp:Label ID="lblNumber" runat="server" /><br />
                Issue Date: <asp:Label ID="lblDate" runat="server" />
            </p>
        </div>
        <p class="text-center no-print">
            <button type="button" class="btn" onclick="printPage()">Print Certificate</button>
        </p>
    </asp:Panel>
</asp:Content>
