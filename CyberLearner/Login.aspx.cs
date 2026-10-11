using System;
using CyberLearner.Models;
using CyberLearner.Security;
using CyberLearner.Services;

namespace CyberLearner
{
    public partial class Login : System.Web.UI.Page
    {
        AuthService authService = new AuthService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (SessionHelper.IsLoggedIn())
            {
                SessionHelper.RedirectToDashboard();
            }

            if (!IsPostBack)
            {
                if (Request.QueryString["registered"] == "1")
                {
                    ShowMessage("Registration successful. You can login now.", "alert alert-success");
                }
                else if (Request.QueryString["reset"] == "1")
                {
                    ShowMessage("Your password has been changed. You can login now.", "alert alert-success");
                }
                else if (Request.QueryString["loggedout"] == "1")
                {
                    ShowMessage("You have been logged out.", "alert alert-info");
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            User user = authService.Login(txtUsername.Text.Trim(), txtPassword.Text);

            if (user == null)
            {
                ShowMessage("Invalid username or password.", "alert alert-danger");
                return;
            }

            if (user.AccountStatus != "Active")
            {
                ShowMessage("Your account is inactive. Please contact management.", "alert alert-danger");
                return;
            }

            SessionHelper.SignIn(user);
            SessionHelper.RedirectToDashboard();
        }

        private void ShowMessage(string message, string cssClass)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = cssClass;
            lblMessage.Visible = true;
        }
    }
}
