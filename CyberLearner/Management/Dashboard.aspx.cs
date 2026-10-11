using System;
using CyberLearner.Data;
using CyberLearner.Services;

namespace CyberLearner.Management
{
    public partial class Dashboard : System.Web.UI.Page
    {
        RequestService requestService = new RequestService();
        CertificateService certificateService = new CertificateService();

        protected void Page_Load(object sender, EventArgs e)
        {
            lblLearners.Text = Count("SELECT COUNT(*) FROM Users WHERE Role = 'Learner'").ToString();
            lblActiveLearners.Text = Count("SELECT COUNT(*) FROM Users WHERE Role = 'Learner' AND AccountStatus = 'Active'").ToString();
            lblTopics.Text = Count("SELECT COUNT(*) FROM Topics WHERE Status = 'Active'").ToString();
            lblQuizQuestions.Text = Count("SELECT COUNT(*) FROM TopicQuizQuestions").ToString();
            lblExamQuestions.Text = Count("SELECT COUNT(*) FROM ExamQuestions").ToString();

            lblPending.Text = requestService.CountPendingRequests().ToString();
            lblCertificates.Text = certificateService.CountCertificates().ToString();

            int attempts = Count("SELECT COUNT(*) FROM ExamAttempts");
            int passed = Count("SELECT COUNT(*) FROM ExamAttempts WHERE PassStatus = 'Pass'");

            lblAttempts.Text = attempts.ToString();
            lblPassed.Text = passed.ToString();

            if (attempts > 0)
            {
                lblPassRate.Text = (passed * 100 / attempts) + "%";
            }
            else
            {
                lblPassRate.Text = "-";
            }

            string sql = "SELECT TOP 5 u.FullName, a.AttemptNumber, a.Score, a.PassStatus, a.AttemptDate " +
                         "FROM ExamAttempts a INNER JOIN Users u ON a.UserID = u.UserID ORDER BY a.AttemptDate DESC";

            gvLatest.DataSource = DatabaseHelper.GetTable(sql);
            gvLatest.DataBind();
        }

        private int Count(string sql)
        {
            return Convert.ToInt32(DatabaseHelper.GetValue(sql));
        }
    }
}
