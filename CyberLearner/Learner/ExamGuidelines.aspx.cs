using System;
using System.Configuration;
using CyberLearner.Security;
using CyberLearner.Services;

namespace CyberLearner.Learner
{
    public partial class ExamGuidelines : System.Web.UI.Page
    {
        ExamService examService = new ExamService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!examService.CanTakeExam(SessionHelper.GetUserID()))
            {
                Response.Redirect("~/Learner/ExamAccess.aspx");
            }

            lblQuestionCount.Text = examService.GetQuestionCount().ToString();
            lblPassMark.Text = examService.GetPassMark().ToString();
            lblWaitDays.Text = ConfigurationManager.AppSettings["RetakeWaitDays"];
        }

        protected void btnStart_Click(object sender, EventArgs e)
        {
            if (!chkAgree.Checked)
            {
                lblMessage.Text = "Please confirm that you have read the guidelines.";
                lblMessage.CssClass = "alert alert-danger";
                lblMessage.Visible = true;
                return;
            }

            Session["ExamGuidelinesAccepted"] = true;
            Response.Redirect("~/Learner/CertificationExam.aspx");
        }
    }
}
