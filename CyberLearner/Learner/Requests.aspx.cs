using System;
using CyberLearner.Models;
using CyberLearner.Security;
using CyberLearner.Services;

namespace CyberLearner.Learner
{
    public partial class Requests : System.Web.UI.Page
    {
        RequestService requestService = new RequestService();
        ExamService examService = new ExamService();
        LearningService learningService = new LearningService();

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Visible = false;

            if (!IsPostBack)
            {
                LoadPage();
            }
        }

        private void LoadPage()
        {
            int userId = SessionHelper.GetUserID();

            lblWaitDays.Text = requestService.GetRetakeWaitDays().ToString();
            btnDirect.Visible = CanRequestDirect(userId);
            btnRetake.Visible = CanRequestRetake(userId);

            gvDirect.DataSource = requestService.GetDirectRequestsByUser(userId);
            gvDirect.DataBind();

            gvRetake.DataSource = requestService.GetRetakeRequestsByUser(userId);
            gvRetake.DataBind();
        }

        private bool CanRequestDirect(int userId)
        {
            if (examService.CountAttempts(userId) > 0)
            {
                lblDirectStatus.Text = "Not needed. You have already attempted the exam.";
                return false;
            }

            if (learningService.HasCompletedCourse(userId))
            {
                lblDirectStatus.Text = "Not needed. You completed the course and already have exam access.";
                return false;
            }

            DirectExamRequest latest = requestService.GetLatestDirectRequest(userId);

            if (latest == null)
            {
                lblDirectStatus.Text = "You have not requested direct exam access.";
                return true;
            }

            if (latest.Status == "Pending")
            {
                lblDirectStatus.Text = "Your request is pending. Please wait for management to review it.";
                return false;
            }

            if (latest.Status == "Approved")
            {
                lblDirectStatus.Text = "Your request was approved. You can go to the Exam page.";
                return false;
            }

            lblDirectStatus.Text = "Your last request was rejected. You can submit a new request.";
            return true;
        }

        private bool CanRequestRetake(int userId)
        {
            ExamAttempt latest = examService.GetLatestAttempt(userId);

            if (latest == null)
            {
                lblRetakeStatus.Text = "You have not taken the exam yet.";
                return false;
            }

            if (examService.GetPassedAttempt(userId) != null)
            {
                lblRetakeStatus.Text = "Not needed. You have passed the exam.";
                return false;
            }

            RetakeRequest request = requestService.GetLatestRetakeRequest(userId, latest.ExamAttemptID);

            if (request != null && request.Status == "Pending")
            {
                lblRetakeStatus.Text = "Your retake request is pending. Please wait for management to review it.";
                return false;
            }

            if (request != null && request.Status == "Approved")
            {
                lblRetakeStatus.Text = "Your retake request was approved. You can go to the Exam page.";
                return false;
            }

            DateTime allowedDate = latest.AttemptDate.AddDays(requestService.GetRetakeWaitDays());

            if (DateTime.Now < allowedDate)
            {
                lblRetakeStatus.Text = "You failed attempt " + latest.AttemptNumber + " on " + latest.AttemptDate.ToString("dd MMM yyyy") +
                                       ". You can request a retake from " + allowedDate.ToString("dd MMM yyyy HH:mm") + ".";
                return false;
            }

            if (request != null && request.Status == "Rejected")
            {
                lblRetakeStatus.Text = "Your last retake request was rejected. You can submit a new request.";
            }
            else
            {
                lblRetakeStatus.Text = "You failed attempt " + latest.AttemptNumber + ". You can now request a retake.";
            }

            return true;
        }

        protected void btnDirect_Click(object sender, EventArgs e)
        {
            int userId = SessionHelper.GetUserID();

            if (CanRequestDirect(userId))
            {
                requestService.CreateDirectRequest(userId);
                ShowMessage("Your direct exam access request has been submitted.", "alert alert-success");
            }
            else
            {
                ShowMessage("You cannot submit a direct exam request right now.", "alert alert-danger");
            }

            LoadPage();
        }

        protected void btnRetake_Click(object sender, EventArgs e)
        {
            int userId = SessionHelper.GetUserID();

            if (CanRequestRetake(userId))
            {
                ExamAttempt latest = examService.GetLatestAttempt(userId);
                requestService.CreateRetakeRequest(userId, latest.ExamAttemptID);
                ShowMessage("Your retake request has been submitted.", "alert alert-success");
            }
            else
            {
                ShowMessage("You cannot submit a retake request right now.", "alert alert-danger");
            }

            LoadPage();
        }

        private void ShowMessage(string message, string cssClass)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = cssClass;
            lblMessage.Visible = true;
        }
    }
}
