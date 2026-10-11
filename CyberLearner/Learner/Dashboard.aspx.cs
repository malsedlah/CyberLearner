using System;
using CyberLearner.Models;
using CyberLearner.Security;
using CyberLearner.Services;

namespace CyberLearner.Learner
{
    public partial class Dashboard : System.Web.UI.Page
    {
        LearningService learningService = new LearningService();
        QuizService quizService = new QuizService();
        ExamService examService = new ExamService();
        RequestService requestService = new RequestService();
        CertificateService certificateService = new CertificateService();

        protected int ProgressPercent = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            int userId = SessionHelper.GetUserID();
            lblName.Text = Server.HtmlEncode(SessionHelper.GetFullName());

            int total = learningService.CountActiveTopics();
            int passed = learningService.CountPassedTopics(userId);
            lblTopics.Text = passed + " / " + total;

            if (total > 0)
            {
                ProgressPercent = passed * 100 / total;
            }

            lblQuizAttempts.Text = quizService.GetAttemptsByUser(userId).Rows.Count.ToString();

            LoadExamStatus(userId);
            LoadRequestStatus(userId);

            Models.Certificate certificate = certificateService.GetCertificateByUser(userId);

            if (certificate != null)
            {
                lblCertificate.Text = "Issued on " + certificate.IssueDate.ToString("dd MMM yyyy") + "<br />Number: " + certificate.CertificateNumber;
            }
            else
            {
                lblCertificate.Text = "Not earned yet. Pass the certification exam to get your certificate.";
            }
        }

        private void LoadExamStatus(int userId)
        {
            ExamAttempt latest = examService.GetLatestAttempt(userId);

            if (examService.GetPassedAttempt(userId) != null)
            {
                lblExamStatus.Text = "Passed";
                lblExamStatus.CssClass = "badge badge-pass";
            }
            else if (examService.CanTakeExam(userId))
            {
                lblExamStatus.Text = "Ready to take";
                lblExamStatus.CssClass = "badge badge-approved";
            }
            else
            {
                lblExamStatus.Text = "Not available yet";
                lblExamStatus.CssClass = "badge badge-locked";
            }

            if (latest != null)
            {
                lblExamInfo.Text = "Attempts: " + latest.AttemptNumber + ". Last score: " + latest.Score.ToString("0.##") + "%";
            }
            else
            {
                lblExamInfo.Text = "You have not attempted the exam.";
            }
        }

        private void LoadRequestStatus(int userId)
        {
            DirectExamRequest direct = requestService.GetLatestDirectRequest(userId);

            if (direct != null)
            {
                lblDirect.Text = direct.Status;
                lblDirect.CssClass = "badge badge-" + direct.Status.ToLower();
            }
            else
            {
                lblDirect.Text = "No request made";
            }

            ExamAttempt latest = examService.GetLatestAttempt(userId);
            RetakeRequest retake = null;

            if (latest != null)
            {
                retake = requestService.GetLatestRetakeRequest(userId, latest.ExamAttemptID);
            }

            if (retake != null)
            {
                lblRetake.Text = retake.Status;
                lblRetake.CssClass = "badge badge-" + retake.Status.ToLower();
            }
            else
            {
                lblRetake.Text = "No request for your latest attempt";
            }
        }
    }
}
