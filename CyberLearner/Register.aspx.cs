using System;
using CyberLearner.Security;
using CyberLearner.Services;

namespace CyberLearner
{
    public partial class Register : System.Web.UI.Page
    {
        AuthService authService = new AuthService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (SessionHelper.IsLoggedIn())
            {
                SessionHelper.RedirectToDashboard();
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            string fullName = txtFullName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

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

            authService.RegisterLearner(fullName, email, username, password);
            Response.Redirect("~/Login.aspx?registered=1");
        }

        private void ShowError(string message)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = "alert alert-danger";
            lblMessage.Visible = true;
        }
    }
}
