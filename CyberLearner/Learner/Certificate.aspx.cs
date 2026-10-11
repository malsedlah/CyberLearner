using System;
using CyberLearner.Models;
using CyberLearner.Security;
using CyberLearner.Services;

namespace CyberLearner.Learner
{
    public partial class Certificate : System.Web.UI.Page
    {
        CertificateService certificateService = new CertificateService();
        ExamService examService = new ExamService();

        protected void Page_Load(object sender, EventArgs e)
        {
            int userId = SessionHelper.GetUserID();
            Models.Certificate certificate = certificateService.IssueCertificate(userId);

            if (certificate == null)
            {
                pnlNoCertificate.Visible = true;
                return;
            }

            ExamAttempt attempt = examService.GetAttempt(certificate.ExamAttemptID);

            lblName.Text = Server.HtmlEncode(SessionHelper.GetFullName());
            lblScore.Text = attempt.Score.ToString("0.##");
            lblNumber.Text = certificate.CertificateNumber;
            lblDate.Text = certificate.IssueDate.ToString("dd MMMM yyyy");
            pnlCertificate.Visible = true;
        }
    }
}
