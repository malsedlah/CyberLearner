using System;
using System.Web.UI.WebControls;
using CyberLearner.Models;
using CyberLearner.Services;

namespace CyberLearner.Management
{
    public partial class TopicQuizzes : System.Web.UI.Page
    {
        LearningService learningService = new LearningService();
        QuizService quizService = new QuizService();

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Visible = false;

            if (!IsPostBack)
            {
                ddlTopic.DataSource = learningService.GetAllTopics();
                ddlTopic.DataTextField = "TopicTitle";
                ddlTopic.DataValueField = "TopicID";
                ddlTopic.DataBind();

                LoadQuestions();
            }
        }

        private void LoadQuestions()
        {
            if (ddlTopic.Items.Count == 0)
            {
                ShowMessage("Please add a topic in Learning Content before adding questions.", "alert alert-info");
                btnSave.Enabled = false;
                return;
            }

            gvQuestions.DataSource = quizService.GetQuestionsByTopic(Convert.ToInt32(ddlTopic.SelectedValue));
            gvQuestions.DataBind();
        }

        protected void ddlTopic_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearForm();
            LoadQuestions();
        }

        protected void gvQuestions_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int questionId = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditQuestion")
            {
                TopicQuizQuestion question = quizService.GetQuestion(questionId);

                if (question != null)
                {
                    hfQuestionID.Value = question.QuestionID.ToString();
                    txtQuestion.Text = question.QuestionText;
                    txtOptionA.Text = question.OptionA;
                    txtOptionB.Text = question.OptionB;
                    txtOptionC.Text = question.OptionC;
                    txtOptionD.Text = question.OptionD;
                    ddlCorrect.SelectedValue = question.CorrectAnswer;
                    ddlStatus.SelectedValue = question.Status;
                    lblFormTitle.Text = "Edit Question " + question.QuestionID;
                }
            }
            else if (e.CommandName == "DeleteQuestion")
            {
                quizService.DeleteQuestion(questionId);
                ClearForm();
                LoadQuestions();
                ShowMessage("The question has been deleted.", "alert alert-success");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            TopicQuizQuestion question = new TopicQuizQuestion();
            question.QuestionID = Convert.ToInt32(hfQuestionID.Value);
            question.TopicID = Convert.ToInt32(ddlTopic.SelectedValue);
            question.QuestionText = txtQuestion.Text.Trim();
            question.OptionA = txtOptionA.Text.Trim();
            question.OptionB = txtOptionB.Text.Trim();
            question.OptionC = txtOptionC.Text.Trim();
            question.OptionD = txtOptionD.Text.Trim();
            question.CorrectAnswer = ddlCorrect.SelectedValue;
            question.Status = ddlStatus.SelectedValue;

            if (question.QuestionText.Length > 500)
            {
                ShowMessage("The question text cannot be longer than 500 characters.", "alert alert-danger");
                return;
            }

            if (question.QuestionID == 0)
            {
                quizService.AddQuestion(question);
                ShowMessage("The question has been added.", "alert alert-success");
            }
            else
            {
                quizService.UpdateQuestion(question);
                ShowMessage("The question has been updated.", "alert alert-success");
            }

            ClearForm();
            LoadQuestions();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            hfQuestionID.Value = "0";
            txtQuestion.Text = "";
            txtOptionA.Text = "";
            txtOptionB.Text = "";
            txtOptionC.Text = "";
            txtOptionD.Text = "";
            ddlCorrect.SelectedValue = "A";
            ddlStatus.SelectedValue = "Active";
            lblFormTitle.Text = "Add New Question";
        }

        private void ShowMessage(string message, string cssClass)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = cssClass;
            lblMessage.Visible = true;
        }
    }
}
