using System;
using CyberLearner.Models;
using CyberLearner.Security;
using CyberLearner.Services;

namespace CyberLearner.Learner
{
    public partial class TopicQuizResult : System.Web.UI.Page
    {
        LearningService learningService = new LearningService();
        QuizService quizService = new QuizService();

        protected void Page_Load(object sender, EventArgs e)
        {
            int attemptId;

            if (!int.TryParse(Request.QueryString["attemptId"], out attemptId))
            {
                Response.Redirect("~/Learner/Topics.aspx");
            }

            TopicQuizAttempt attempt = quizService.GetAttempt(attemptId);

            if (attempt == null || attempt.UserID != SessionHelper.GetUserID())
            {
                Response.Redirect("~/Learner/Topics.aspx");
            }

            Topic topic = learningService.GetTopic(attempt.TopicID);

            lblTopic.Text = Server.HtmlEncode(topic.TopicTitle);
            lblScore.Text = attempt.Score.ToString("0.##");
            lblPassMark.Text = quizService.GetPassMark().ToString();
            lblDate.Text = attempt.AttemptDate.ToString("dd MMM yyyy HH:mm");
            lblStatus.Text = attempt.PassStatus;
            lblStatus.CssClass = "badge badge-" + attempt.PassStatus.ToLower();
            lnkMaterial.NavigateUrl = "LearningMaterial.aspx?topicId=" + attempt.TopicID;

            if (attempt.PassStatus == "Pass")
            {
                int nextTopicId = learningService.GetNextTopicId(attempt.TopicID);

                if (nextTopicId != -1)
                {
                    lblInfo.Text = "Well done! The next topic is now unlocked.";
                    lnkNext.Text = "Go to Next Topic";
                    lnkNext.NavigateUrl = "LearningMaterial.aspx?topicId=" + nextTopicId;
                }
                else
                {
                    lblInfo.Text = "Well done! You have finished the last topic.";
                    lnkNext.Text = "Go to Certification Exam";
                    lnkNext.NavigateUrl = "ExamAccess.aspx";
                }

                lnkNext.Visible = true;
            }
            else
            {
                lblInfo.Text = "You did not reach the pass mark. Review the learning material and try again.";
                lnkRetry.NavigateUrl = "TopicQuiz.aspx?topicId=" + attempt.TopicID;
                lnkRetry.Visible = true;
            }
        }
    }
}
