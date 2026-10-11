using System;
using System.Web.UI.WebControls;
using CyberLearner.Models;
using CyberLearner.Services;

namespace CyberLearner.Management
{
    public partial class CertificationExam : System.Web.UI.Page
    {
        ExamService examService = new ExamService();

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Visible = false;

            if (!IsPostBack)
            {
                LoadQuestions();
            }
        }

        private void LoadQuestions()
        {
            gvQuestions.DataSource = examService.GetAllQuestions();
            gvQuestions.DataBind();

            lblActiveCount.Text = examService.CountActiveQuestions().ToString();
            lblExamCount.Text = examService.GetQuestionCount().ToString();
        }

        protected void gvQuestions_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int questionId = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditQuestion")
            {
                ExamQuestion question = examService.GetQuestion(questionId);

                if (question != null)
                {
                    hfQuestionID.Value = question.ExamQuestionID.ToString();
                    txtQuestion.Text = question.QuestionText;
                    txtOptionA.Text = question.OptionA;
                    txtOptionB.Text = question.OptionB;
                    txtOptionC.Text = question.OptionC;
                    txtOptionD.Text = question.OptionD;
                    ddlCorrect.SelectedValue = question.CorrectAnswer;
                    ddlStatus.SelectedValue = question.Status;
                    lblFormTitle.Text = "Edit Question " + question.ExamQuestionID;
                }
            }
            else if (e.CommandName == "DeleteQuestion")
            {
                examService.DeleteQuestion(questionId);
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

            ExamQuestion question = new ExamQuestion();
            question.ExamQuestionID = Convert.ToInt32(hfQuestionID.Value);
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

            if (question.ExamQuestionID == 0)
            {
                examService.AddQuestion(question);
                ShowMessage("The question has been added.", "alert alert-success");
            }
            else
            {
                examService.UpdateQuestion(question);
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
