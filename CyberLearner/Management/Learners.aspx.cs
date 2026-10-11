using System;
using System.Web.UI.WebControls;
using CyberLearner.Models;
using CyberLearner.Services;

namespace CyberLearner.Management
{
    public partial class Learners : System.Web.UI.Page
    {
        UserService userService = new UserService();

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Visible = false;

            if (!IsPostBack)
            {
                LoadLearners();
            }
        }

        private void LoadLearners()
        {
            gvLearners.DataSource = userService.GetLearners(txtSearch.Text.Trim());
            gvLearners.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadLearners();
        }

        protected void btnShowAll_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            LoadLearners();
        }

        protected void gvLearners_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int userId = Convert.ToInt32(e.CommandArgument);
            User user = userService.GetUserById(userId);

            if (user == null || user.Role != "Learner")
            {
                return;
            }

            if (e.CommandName == "EditLearner")
            {
                hfUserID.Value = user.UserID.ToString();
                txtFullName.Text = user.FullName;
                txtEmail.Text = user.Email;
                pnlEdit.Visible = true;
            }
            else if (e.CommandName == "ChangeStatus")
            {
                if (user.AccountStatus == "Active")
                {
                    userService.SetAccountStatus(userId, "Inactive");
                    ShowMessage(user.FullName + " has been deactivated.", "alert alert-success");
                }
                else
                {
                    userService.SetAccountStatus(userId, "Active");
                    ShowMessage(user.FullName + " has been activated.", "alert alert-success");
                }

                LoadLearners();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            int userId = Convert.ToInt32(hfUserID.Value);
            string fullName = txtFullName.Text.Trim();
            string email = txtEmail.Text.Trim();

            if (userService.EmailUsedByOther(email, userId))
            {
                ShowMessage("This email is already used by another account.", "alert alert-danger");
                return;
            }

            userService.UpdateProfile(userId, fullName, email);
            pnlEdit.Visible = false;
            LoadLearners();
            ShowMessage("Learner details have been updated.", "alert alert-success");
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pnlEdit.Visible = false;
        }

        private void ShowMessage(string message, string cssClass)
        {
            lblMessage.Text = Server.HtmlEncode(message);
            lblMessage.CssClass = cssClass;
            lblMessage.Visible = true;
        }
    }
}
