<%@ Page Title="Learner Results" Language="C#" MasterPageFile="~/Management.Master" AutoEventWireup="true" CodeBehind="LearnerResults.aspx.cs" Inherits="CyberLearner.Management.LearnerResults" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="page-title">Learner Results</h1>

    <div class="search-row">
        <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Search learner name or username" MaxLength="50" />
        <asp:DropDownList ID="ddlResult" runat="server" CssClass="form-control">
            <asp:ListItem Text="All results" Value="All" />
            <asp:ListItem Text="Pass only" Value="Pass" />
            <asp:ListItem Text="Fail only" Value="Fail" />
        </asp:DropDownList>
        <asp:Button ID="btnSearch" runat="server" Text="Filter" CssClass="btn" OnClick="btnSearch_Click" />
        <asp:Button ID="btnShowAll" runat="server" Text="Show All" CssClass="btn btn-secondary" OnClick="btnShowAll_Click" />
    </div>

    <div class="card">
        <h2>Certification Exam Results</h2>
        <div class="table-wrap">
            <asp:GridView ID="gvExam" runat="server" CssClass="table" AutoGenerateColumns="false" GridLines="None"
                EmptyDataText="No exam attempts found.">
                <Columns>
                    <asp:BoundField DataField="FullName" HeaderText="Learner" />
                    <asp:BoundField DataField="Username" HeaderText="Username" />
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
    </div>

    <div class="card">
        <h2>Topic Quiz Results</h2>
        <div class="table-wrap">
            <asp:GridView ID="gvQuiz" runat="server" CssClass="table" AutoGenerateColumns="false" GridLines="None"
                EmptyDataText="No quiz attempts found.">
                <Columns>
                    <asp:BoundField DataField="FullName" HeaderText="Learner" />
                    <asp:BoundField DataField="Username" HeaderText="Username" />
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
