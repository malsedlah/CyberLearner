using System;
using System.Configuration;
using CyberLearner.Models;
using CyberLearner.Services;

namespace CyberLearner
{
    public partial class ForgotPassword : System.Web.UI.Page
    {
        AuthService authService = new AuthService();

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
        }

        protected void btnSendCode_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            User user = authService.GetUserByEmail(txtEmail.Text.Trim());

            if (user == null)
            {
                ShowMessage("No account was found with this email.", "alert alert-danger");
                return;
            }

            int minutes = Convert.ToInt32(ConfigurationManager.AppSettings["ResetCodeMinutes"]);
            Random random = new Random();
            string code = random.Next(100000, 1000000).ToString();

            Session["ResetUserID"] = user.UserID;
            Session["ResetCode"] = code;
            Session["ResetExpiry"] = DateTime.Now.AddMinutes(minutes);
            Session["ResetVerified"] = false;

            lblCode.Text = code;
            pnlEmail.Visible = false;
            pnlCode.Visible = true;
            ShowMessage("A verification code was created. It expires in " + minutes + " minutes.", "alert alert-success");
        }

        protected void btnVerify_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            if (Session["ResetCode"] == null)
            {
                StartAgain("Your reset session has ended. Please start again.");
                return;
            }

            DateTime expiry = (DateTime)Session["ResetExpiry"];

            if (DateTime.Now > expiry)
            {
                StartAgain("The verification code has expired. Please request a new code.");
                return;
            }

            if (txtCode.Text.Trim() != Session["ResetCode"].ToString())
            {
                ShowMessage("The verification code is incorrect.", "alert alert-danger");
                return;
            }

            Session["ResetVerified"] = true;
            pnlCode.Visible = false;
            pnlPassword.Visible = true;
            ShowMessage("Code verified. Please enter a new password.", "alert alert-success");
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            if (Session["ResetUserID"] == null || Session["ResetVerified"] == null || !(bool)Session["ResetVerified"])
            {
                StartAgain("Your reset session has ended. Please start again.");
                return;
            }

            int userId = Convert.ToInt32(Session["ResetUserID"]);
            authService.UpdatePassword(userId, txtPassword.Text);

            ClearResetSession();
            Response.Redirect("~/Login.aspx?reset=1");
        }

        private void StartAgain(string message)
        {
            ClearResetSession();
            pnlEmail.Visible = true;
            pnlCode.Visible = false;
            pnlPassword.Visible = false;
            ShowMessage(message, "alert alert-danger");
        }

        private void ClearResetSession()
        {
            Session.Remove("ResetUserID");
            Session.Remove("ResetCode");
            Session.Remove("ResetExpiry");
            Session.Remove("ResetVerified");
        }

        private void ShowMessage(string message, string cssClass)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = cssClass;
            lblMessage.Visible = true;
        }
    }
}
