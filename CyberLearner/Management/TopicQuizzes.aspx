<%@ Page Title="Topic Quizzes" Language="C#" MasterPageFile="~/Management.Master" AutoEventWireup="true" CodeBehind="TopicQuizzes.aspx.cs" Inherits="CyberLearner.Management.TopicQuizzes" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="page-title">Topic Quiz Questions</h1>
    <asp:Label ID="lblMessage" runat="server" Visible="false" />

    <div class="search-row">
        <label for="<%= ddlTopic.ClientID %>">Topic:</label>
        <asp:DropDownList ID="ddlTopic" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlTopic_SelectedIndexChanged" CausesValidation="false" />
    </div>

    <div class="table-wrap">
        <asp:GridView ID="gvQuestions" runat="server" CssClass="table" AutoGenerateColumns="false" GridLines="None"
            EmptyDataText="This topic has no quiz questions yet." OnRowCommand="gvQuestions_RowCommand">
            <Columns>
                <asp:BoundField DataField="QuestionID" HeaderText="ID" />
                <asp:BoundField DataField="QuestionText" HeaderText="Question" />
                <asp:BoundField DataField="CorrectAnswer" HeaderText="Answer" />
                <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>
                        <span class='badge badge-<%# Eval("Status").ToString().ToLower() %>'><%# Eval("Status") %></span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Actions">
                    <ItemTemplate>
                        <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btn btn-small" CommandName="EditQuestion" CommandArgument='<%# Eval("QuestionID") %>' CausesValidation="false" />
                        <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btn btn-small btn-danger" CommandName="DeleteQuestion" CommandArgument='<%# Eval("QuestionID") %>' CausesValidation="false" OnClientClick="return confirmDelete();" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>

    <div class="card" style="margin-top: 20px;">
        <h2><asp:Label ID="lblFormTitle" runat="server" Text="Add New Question" /></h2>
        <asp:HiddenField ID="hfQuestionID" runat="server" Value="0" />

        <div class="form-group">
            <label for="<%= txtQuestion.ClientID %>">Question</label>
            <asp:TextBox ID="txtQuestion" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" MaxLength="500" />
            <asp:RequiredFieldValidator ID="rfvQuestion" runat="server" ControlToValidate="txtQuestion" ErrorMessage="Question text is required." CssClass="error" Display="Dynamic" />
        </div>
        <div class="form-group">
            <label for="<%= txtOptionA.ClientID %>">Option A</label>
            <asp:TextBox ID="txtOptionA" runat="server" CssClass="form-control" MaxLength="300" />
            <asp:RequiredFieldValidator ID="rfvOptionA" runat="server" ControlToValidate="txtOptionA" ErrorMessage="Option A is required." CssClass="error" Display="Dynamic" />
        </div>
        <div class="form-group">
            <label for="<%= txtOptionB.ClientID %>">Option B</label>
            <asp:TextBox ID="txtOptionB" runat="server" CssClass="form-control" MaxLength="300" />
            <asp:RequiredFieldValidator ID="rfvOptionB" runat="server" ControlToValidate="txtOptionB" ErrorMessage="Option B is required." CssClass="error" Display="Dynamic" />
        </div>
        <div class="form-group">
            <label for="<%= txtOptionC.ClientID %>">Option C</label>
            <asp:TextBox ID="txtOptionC" runat="server" CssClass="form-control" MaxLength="300" />
            <asp:RequiredFieldValidator ID="rfvOptionC" runat="server" ControlToValidate="txtOptionC" ErrorMessage="Option C is required." CssClass="error" Display="Dynamic" />
        </div>
        <div class="form-group">
            <label for="<%= txtOptionD.ClientID %>">Option D</label>
            <asp:TextBox ID="txtOptionD" runat="server" CssClass="form-control" MaxLength="300" />
            <asp:RequiredFieldValidator ID="rfvOptionD" runat="server" ControlToValidate="txtOptionD" ErrorMessage="Option D is required." CssClass="error" Display="Dynamic" />
        </div>
        <div class="form-group">
            <label for="<%= ddlCorrect.ClientID %>">Correct Answer</label>
            <asp:DropDownList ID="ddlCorrect" runat="server" CssClass="form-control">
                <asp:ListItem Text="A" Value="A" />
                <asp:ListItem Text="B" Value="B" />
                <asp:ListItem Text="C" Value="C" />
                <asp:ListItem Text="D" Value="D" />
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <label for="<%= ddlStatus.ClientID %>">Status</label>
            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                <asp:ListItem Text="Active" Value="Active" />
                <asp:ListItem Text="Inactive" Value="Inactive" />
            </asp:DropDownList>
        </div>

        <asp:Button ID="btnSave" runat="server" Text="Save Question" CssClass="btn" OnClick="btnSave_Click" />
        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-secondary" OnClick="btnClear_Click" CausesValidation="false" />
    </div>
</asp:Content>
