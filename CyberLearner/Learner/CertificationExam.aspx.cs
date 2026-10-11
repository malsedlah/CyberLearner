using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using CyberLearner.Models;
using CyberLearner.Security;
using CyberLearner.Services;

namespace CyberLearner.Learner
{
    public partial class CertificationExam : System.Web.UI.Page
    {
        ExamService examService = new ExamService();

        List<ExamQuestion> questions;
        string[] answers;

        protected void Page_Load(object sender, EventArgs e)
        {
            int userId = SessionHelper.GetUserID();

            if (!examService.CanTakeExam(userId) || Session["ExamGuidelinesAccepted"] == null)
            {
                Response.Redirect("~/Learner/ExamAccess.aspx");
            }

            lblMessage.Visible = false;

            if (!IsPostBack)
            {
                questions = examService.GetExamQuestions();

                if (questions.Count == 0)
                {
                    pnlExam.Visible = false;
                    lblMessage.Text = "The exam has no questions yet. Please try again later.";
                    lblMessage.CssClass = "alert alert-info";
                    lblMessage.Visible = true;
                    return;
                }

                answers = new string[questions.Count];
                Session["ExamQuestions"] = questions;
                Session["ExamAnswers"] = answers;
                ViewState["Index"] = 0;
                ShowQuestion();
            }
            else
            {
                if (Session["ExamQuestions"] == null)
                {
                    Response.Redirect("~/Learner/ExamAccess.aspx");
                }

                questions = (List<ExamQuestion>)Session["ExamQuestions"];
                answers = (string[])Session["ExamAnswers"];
            }
        }

        private void ShowQuestion()
        {
            int index = (int)ViewState["Index"];
            ExamQuestion question = questions[index];

            lblProgress.Text = "Question " + (index + 1) + " of " + questions.Count;
            lblQuestion.Text = Server.HtmlEncode(question.QuestionText);

            rblOptions.Items.Clear();
            rblOptions.Items.Add(new ListItem("A. " + Server.HtmlEncode(question.OptionA), "A"));
            rblOptions.Items.Add(new ListItem("B. " + Server.HtmlEncode(question.OptionB), "B"));
            rblOptions.Items.Add(new ListItem("C. " + Server.HtmlEncode(question.OptionC), "C"));
            rblOptions.Items.Add(new ListItem("D. " + Server.HtmlEncode(question.OptionD), "D"));

            if (answers[index] != null)
            {
                rblOptions.SelectedValue = answers[index];
            }

            btnPrevious.Visible = index > 0;
            btnNext.Visible = index < questions.Count - 1;
            btnSubmit.Visible = index == questions.Count - 1;
        }

        private void SaveAnswer()
        {
            int index = (int)ViewState["Index"];

            if (rblOptions.SelectedValue != "")
            {
                answers[index] = rblOptions.SelectedValue;
                Session["ExamAnswers"] = answers;
            }
        }

        protected void btnPrevious_Click(object sender, EventArgs e)
        {
            SaveAnswer();
            ViewState["Index"] = (int)ViewState["Index"] - 1;
            ShowQuestion();
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (rblOptions.SelectedValue == "")
            {
                ShowError("Please select an answer before continuing.");
                return;
            }

            SaveAnswer();
            ViewState["Index"] = (int)ViewState["Index"] + 1;
            ShowQuestion();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            SaveAnswer();

            int correct = 0;

            for (int i = 0; i < questions.Count; i++)
            {
                if (answers[i] == null)
                {
                    ShowError("Please answer all questions before submitting. Question " + (i + 1) + " is not answered.");
                    return;
                }

                if (answers[i] == questions[i].CorrectAnswer)
                {
                    correct++;
                }
            }

            decimal score = Math.Round((decimal)correct * 100 / questions.Count, 2);
            string passStatus = "Fail";

            if (score >= examService.GetPassMark())
            {
                passStatus = "Pass";
            }

            int attemptId = examService.SaveAttempt(SessionHelper.GetUserID(), score, passStatus);

            Session.Remove("ExamQuestions");
            Session.Remove("ExamAnswers");
            Session.Remove("ExamGuidelinesAccepted");

            Response.Redirect("~/Learner/ExamResult.aspx?attemptId=" + attemptId);
        }

        private void ShowError(string message)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = "alert alert-danger";
            lblMessage.Visible = true;
        }
    }
}
