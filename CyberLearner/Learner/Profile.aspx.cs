using System;
using CyberLearner.Models;
using CyberLearner.Security;
using CyberLearner.Services;

namespace CyberLearner.Learner
{
    public partial class Profile : System.Web.UI.Page
    {
        UserService userService = new UserService();
        AuthService authService = new AuthService();

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Visible = false;

            if (!IsPostBack)
            {
                LoadProfile();
            }
        }

        private void LoadProfile()
        {
            User user = userService.GetUserById(SessionHelper.GetUserID());

            txtUsername.Text = user.Username;
            txtCreatedAt.Text = user.CreatedAt.ToString("dd MMM yyyy");
            txtFullName.Text = user.FullName;
            txtEmail.Text = user.Email;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            int userId = SessionHelper.GetUserID();
            string fullName = txtFullName.Text.Trim();
            string email = txtEmail.Text.Trim();

            if (userService.EmailUsedByOther(email, userId))
            {
                ShowMessage("This email is already used by another account.", "alert alert-danger");
                return;
            }

            userService.UpdateProfile(userId, fullName, email);
            Session["FullName"] = fullName;

            ShowMessage("Your profile has been updated.", "alert alert-success");
        }

        protected void btnPassword_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            User user = userService.GetUserById(SessionHelper.GetUserID());

            if (!PasswordHelper.VerifyPassword(txtCurrent.Text, user.PasswordHash))
            {
                ShowMessage("Your current password is incorrect.", "alert alert-danger");
                return;
            }

            authService.UpdatePassword(user.UserID, txtNew.Text);
            ShowMessage("Your password has been changed.", "alert alert-success");
        }

        private void ShowMessage(string message, string cssClass)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = cssClass;
            lblMessage.Visible = true;
        }
    }
}
