using System;
using System.Configuration;
using CyberLearner.Models;
using CyberLearner.Security;
using CyberLearner.Services;

namespace CyberLearner.Learner
{
    public partial class ExamResult : System.Web.UI.Page
    {
        ExamService examService = new ExamService();

        protected void Page_Load(object sender, EventArgs e)
        {
            int attemptId;

            if (!int.TryParse(Request.QueryString["attemptId"], out attemptId))
            {
                Response.Redirect("~/Learner/Results.aspx");
            }

            ExamAttempt attempt = examService.GetAttempt(attemptId);

            if (attempt == null || attempt.UserID != SessionHelper.GetUserID())
            {
                Response.Redirect("~/Learner/Results.aspx");
            }

            lblAttemptNumber.Text = attempt.AttemptNumber.ToString();
            lblScore.Text = attempt.Score.ToString("0.##");
            lblPassMark.Text = examService.GetPassMark().ToString();
            lblDate.Text = attempt.AttemptDate.ToString("dd MMM yyyy HH:mm");
            lblStatus.Text = attempt.PassStatus;
            lblStatus.CssClass = "badge badge-" + attempt.PassStatus.ToLower();

            if (attempt.PassStatus == "Pass")
            {
                lblInfo.Text = "Congratulations! You passed the exam and earned the Cybersecurity Fundamentals Certificate.";
                lnkCertificate.Visible = true;
            }
            else
            {
                int waitDays = Convert.ToInt32(ConfigurationManager.AppSettings["RetakeWaitDays"]);

                lblInfo.Text = "You did not reach the pass mark. You can submit a retake request from " +
                               attempt.AttemptDate.AddDays(waitDays).ToString("dd MMM yyyy") + ".";
                lnkRequests.Visible = true;
            }
        }
    }
}
