using System;
using System.Configuration;
using System.Security.Cryptography;
using CyberLearner.Security;
using CyberLearner.Services;

namespace CyberLearner
{
    public partial class Register : System.Web.UI.Page
    {
        AuthService authService = new AuthService();
        EmailService emailService = new EmailService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (SessionHelper.IsLoggedIn())
            {
                SessionHelper.RedirectToDashboard();
            }

            lblMessage.Visible = false;
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            string email = txtEmail.Text.Trim();
            string username = txtUsername.Text.Trim();

            if (authService.UsernameExists(username))
            {
                ShowError("This username is already taken.");
                return;
            }

            if (authService.EmailExists(email))
            {
                ShowError("This email is already registered.");
                return;
            }

            Session["PendingFullName"] = txtFullName.Text.Trim();
            Session["PendingEmail"] = email;
            Session["PendingUsername"] = username;
            Session["PendingPasswordHash"] = PasswordHelper.HashPassword(txtPassword.Text);

            if (SendCode())
            {
                lblOtpEmail.Text = Server.HtmlEncode(email);
                pnlForm.Visible = false;
                pnlOtp.Visible = true;
            }
        }

        protected void btnVerify_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            if (Session["PendingCode"] == null)
            {
                StartAgain("Your registration session has ended. Please fill in the form again.");
                return;
            }

            if (DateTime.Now > (DateTime)Session["PendingExpiry"])
            {
                ShowError("The code has expired. Click Resend Code to get a new one.");
                return;
            }

            int attempts = (int)Session["PendingAttempts"] + 1;
            Session["PendingAttempts"] = attempts;

            if (attempts > 5)
            {
                StartAgain("Too many wrong codes. Please register again.");
                return;
            }

            if (txtOtp.Text.Trim() != Session["PendingCode"].ToString())
            {
                ShowError("The code is incorrect. You have " + (5 - attempts) + " tries left.");
                return;
            }

            string email = Session["PendingEmail"].ToString();
            string username = Session["PendingUsername"].ToString();

            if (authService.UsernameExists(username) || authService.EmailExists(email))
            {
                StartAgain("This username or email was registered by someone else. Please try again.");
                return;
            }

            authService.CreateLearner(Session["PendingFullName"].ToString(), email, username, Session["PendingPasswordHash"].ToString());
            ClearPending();
            Response.Redirect("~/Login.aspx?registered=1");
        }

        protected void btnResend_Click(object sender, EventArgs e)
        {
            if (Session["PendingEmail"] == null)
            {
                StartAgain("Your registration session has ended. Please fill in the form again.");
                return;
            }

            if (SendCode())
            {
                lblMessage.Text = "A new code has been sent to your email.";
                lblMessage.CssClass = "alert alert-success";
                lblMessage.Visible = true;
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            ClearPending();
            pnlOtp.Visible = false;
            pnlForm.Visible = true;
        }

        private bool SendCode()
        {
            byte[] bytes = new byte[4];
            new RNGCryptoServiceProvider().GetBytes(bytes);
            string code = (BitConverter.ToUInt32(bytes, 0) % 900000 + 100000).ToString();

            int minutes = Convert.ToInt32(ConfigurationManager.AppSettings["OtpMinutes"]);

            Session["PendingCode"] = code;
            Session["PendingExpiry"] = DateTime.Now.AddMinutes(minutes);
            Session["PendingAttempts"] = 0;

            try
            {
                emailService.SendVerificationCode(Session["PendingEmail"].ToString(), Session["PendingFullName"].ToString(), code);
                return true;
            }
            catch (Exception)
            {
                ShowError("We could not send the verification email. Please check the email address and try again.");
                return false;
            }
        }

        private void StartAgain(string message)
        {
            ClearPending();
            pnlOtp.Visible = false;
            pnlForm.Visible = true;
            ShowError(message);
        }

        private void ClearPending()
        {
            Session.Remove("PendingFullName");
            Session.Remove("PendingEmail");
            Session.Remove("PendingUsername");
            Session.Remove("PendingPasswordHash");
            Session.Remove("PendingCode");
            Session.Remove("PendingExpiry");
            Session.Remove("PendingAttempts");
        }

        private void ShowError(string message)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = "alert alert-danger";
            lblMessage.Visible = true;
        }
    }
}