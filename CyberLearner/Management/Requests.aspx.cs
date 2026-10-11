using System;
using System.Web.UI.WebControls;
using CyberLearner.Security;
using CyberLearner.Services;

namespace CyberLearner.Management
{
    public partial class Requests : System.Web.UI.Page
    {
        RequestService requestService = new RequestService();

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Visible = false;

            if (!IsPostBack)
            {
                LoadRequests();
            }
        }

        private void LoadRequests()
        {
            gvDirect.DataSource = requestService.GetDirectRequests(ddlStatus.SelectedValue);
            gvDirect.DataBind();

            gvRetake.DataSource = requestService.GetRetakeRequests(ddlStatus.SelectedValue);
            gvRetake.DataBind();
        }

        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadRequests();
        }

        protected void gvDirect_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int requestId = Convert.ToInt32(e.CommandArgument);
            int reviewerId = SessionHelper.GetUserID();

            if (e.CommandName == "ApproveRequest")
            {
                requestService.ReviewDirectRequest(requestId, "Approved", reviewerId);
                ShowMessage("The direct exam request has been approved.");
            }
            else if (e.CommandName == "RejectRequest")
            {
                requestService.ReviewDirectRequest(requestId, "Rejected", reviewerId);
                ShowMessage("The direct exam request has been rejected.");
            }

            LoadRequests();
        }

        protected void gvRetake_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int requestId = Convert.ToInt32(e.CommandArgument);
            int reviewerId = SessionHelper.GetUserID();

            if (e.CommandName == "ApproveRequest")
            {
                requestService.ReviewRetakeRequest(requestId, "Approved", reviewerId);
                ShowMessage("The retake request has been approved.");
            }
            else if (e.CommandName == "RejectRequest")
            {
                requestService.ReviewRetakeRequest(requestId, "Rejected", reviewerId);
                ShowMessage("The retake request has been rejected.");
            }

            LoadRequests();
        }

        private void ShowMessage(string message)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = "alert alert-success";
            lblMessage.Visible = true;
        }
    }
}
