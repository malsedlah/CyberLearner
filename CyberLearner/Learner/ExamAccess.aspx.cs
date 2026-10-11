using System;
using System.Configuration;
using CyberLearner.Models;
using CyberLearner.Security;
using CyberLearner.Services;

namespace CyberLearner.Learner
{
    public partial class ExamAccess : System.Web.UI.Page
    {
        ExamService examService = new ExamService();

        protected void Page_Load(object sender, EventArgs e)
        {
            int userId = SessionHelper.GetUserID();
            string status = examService.GetAccessStatus(userId);

            lblQuestionCount.Text = examService.GetQuestionCount().ToString();
            lblPassMark.Text = examService.GetPassMark().ToString();
            lblAttempts.Text = examService.CountAttempts(userId).ToString();

            if (status == "Passed")
            {
                lblStatus.Text = "Passed";
                lblStatus.CssClass = "badge badge-pass";
                lblDetails.Text = "Congratulations! You have already passed the certification exam.";
                lnkCertificate.Visible = true;
                lnkResults.Visible = true;
            }
            else if (status == "EligibleCourse")
            {
                lblStatus.Text = "Eligible";
                lblStatus.CssClass = "badge badge-approved";
                lblDetails.Text = "You have completed all course topics, so you can take the exam.";
                lnkGuidelines.Visible = true;
            }
            else if (status == "EligibleDirect")
            {
                lblStatus.Text = "Eligible";
                lblStatus.CssClass = "badge badge-approved";
                lblDetails.Text = "Your direct exam access request was approved, so you can take the exam.";
                lnkGuidelines.Visible = true;
            }
            else if (status == "EligibleRetake")
            {
                lblStatus.Text = "Eligible";
                lblStatus.CssClass = "badge badge-approved";
                lblDetails.Text = "Your retake request was approved. You have one new attempt.";
                lnkGuidelines.Visible = true;
            }
            else if (status == "RetakeRequired")
            {
                ExamAttempt latest = examService.GetLatestAttempt(userId);
                int waitDays = Convert.ToInt32(ConfigurationManager.AppSettings["RetakeWaitDays"]);

                lblStatus.Text = "Not Eligible";
                lblStatus.CssClass = "badge badge-fail";
                lblDetails.Text = "You failed your last attempt on " + latest.AttemptDate.ToString("dd MMM yyyy") +
                                  ". You can submit a retake request from " + latest.AttemptDate.AddDays(waitDays).ToString("dd MMM yyyy") +
                                  " and it must be approved by management.";
                lnkRequests.Visible = true;
                lnkResults.Visible = true;
            }
            else
            {
                lblStatus.Text = "Not Eligible";
                lblStatus.CssClass = "badge badge-fail";
                lblDetails.Text = "You need to pass all topic quizzes before taking the exam. " +
                                  "If you already know the material you can request direct exam access.";
                lnkCourse.Visible = true;
                lnkRequests.Visible = true;
            }
        }
    }
}
