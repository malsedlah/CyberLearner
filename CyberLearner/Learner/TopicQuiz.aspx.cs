using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using CyberLearner.Models;
using CyberLearner.Security;
using CyberLearner.Services;

namespace CyberLearner.Learner
{
    public partial class TopicQuiz : System.Web.UI.Page
    {
        LearningService learningService = new LearningService();
        QuizService quizService = new QuizService();

        int topicId;
        List<TopicQuizQuestion> questions;
        string[] answers;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(Request.QueryString["topicId"], out topicId))
            {
                Response.Redirect("~/Learner/Topics.aspx");
            }

            int userId = SessionHelper.GetUserID();
            Topic topic = learningService.GetTopic(topicId);

            if (topic == null || topic.Status != "Active" || !learningService.IsTopicUnlocked(userId, topicId))
            {
                Response.Redirect("~/Learner/Topics.aspx");
            }

            lblTopic.Text = Server.HtmlEncode(topic.TopicTitle);
            lnkBack.NavigateUrl = "LearningMaterial.aspx?topicId=" + topicId;
            lblMessage.Visible = false;

            if (!IsPostBack)
            {
                questions = quizService.GetQuizQuestions(topicId);

                if (questions.Count == 0)
                {
                    pnlQuiz.Visible = false;
                    ShowMessage("There are no quiz questions for this topic yet.", "alert alert-info");
                    return;
                }

                answers = new string[questions.Count];
                Session["QuizTopicID"] = topicId;
                Session["QuizQuestions"] = questions;
                Session["QuizAnswers"] = answers;
                ViewState["Index"] = 0;
                ShowQuestion();
            }
            else
            {
                if (Session["QuizQuestions"] == null || Convert.ToInt32(Session["QuizTopicID"]) != topicId)
                {
                    Response.Redirect("~/Learner/TopicQuiz.aspx?topicId=" + topicId);
                }

                questions = (List<TopicQuizQuestion>)Session["QuizQuestions"];
                answers = (string[])Session["QuizAnswers"];
            }
        }

        private void ShowQuestion()
        {
            int index = (int)ViewState["Index"];
            TopicQuizQuestion question = questions[index];

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
                Session["QuizAnswers"] = answers;
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
                ShowMessage("Please select an answer before continuing.", "alert alert-danger");
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
                    ShowMessage("Please answer all questions before submitting. Question " + (i + 1) + " is not answered.", "alert alert-danger");
                    return;
                }

                if (answers[i] == questions[i].CorrectAnswer)
                {
                    correct++;
                }
            }

            decimal score = Math.Round((decimal)correct * 100 / questions.Count, 2);
            string passStatus = "Fail";

            if (score >= quizService.GetPassMark())
            {
                passStatus = "Pass";
            }

            int attemptId = quizService.SaveAttempt(SessionHelper.GetUserID(), topicId, score, passStatus);

            Session.Remove("QuizTopicID");
            Session.Remove("QuizQuestions");
            Session.Remove("QuizAnswers");

            Response.Redirect("~/Learner/TopicQuizResult.aspx?attemptId=" + attemptId);
        }

        private void ShowMessage(string message, string cssClass)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = cssClass;
            lblMessage.Visible = true;
        }
    }
}
