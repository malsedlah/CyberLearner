using System;
using System.Web.UI.WebControls;
using CyberLearner.Models;
using CyberLearner.Services;

namespace CyberLearner.Management
{
    public partial class LearningContent : System.Web.UI.Page
    {
        LearningService learningService = new LearningService();

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Visible = false;

            if (!IsPostBack)
            {
                LoadTopics();
            }
        }

        private void LoadTopics()
        {
            gvTopics.DataSource = learningService.GetAllTopics();
            gvTopics.DataBind();
        }

        protected void gvTopics_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditTopic")
            {
                Topic topic = learningService.GetTopic(Convert.ToInt32(e.CommandArgument));

                if (topic != null)
                {
                    hfTopicID.Value = topic.TopicID.ToString();
                    txtTitle.Text = topic.TopicTitle;
                    txtOrder.Text = topic.TopicOrder.ToString();
                    ddlStatus.SelectedValue = topic.Status;
                    txtContent.Text = topic.TopicContent;
                    lblFormTitle.Text = "Edit Topic";
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            Topic topic = new Topic();
            topic.TopicID = Convert.ToInt32(hfTopicID.Value);
            topic.TopicTitle = txtTitle.Text.Trim();
            topic.TopicOrder = Convert.ToInt32(txtOrder.Text);
            topic.Status = ddlStatus.SelectedValue;
            topic.TopicContent = txtContent.Text.Trim();

            if (learningService.TopicOrderExists(topic.TopicOrder, topic.TopicID))
            {
                ShowMessage("Another topic already uses this order number.", "alert alert-danger");
                return;
            }

            if (topic.TopicID == 0)
            {
                learningService.AddTopic(topic);
                ShowMessage("The topic has been added.", "alert alert-success");
            }
            else
            {
                learningService.UpdateTopic(topic);
                ShowMessage("The topic has been updated.", "alert alert-success");
            }

            ClearForm();
            LoadTopics();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            hfTopicID.Value = "0";
            txtTitle.Text = "";
            txtOrder.Text = "";
            txtContent.Text = "";
            ddlStatus.SelectedValue = "Active";
            lblFormTitle.Text = "Add New Topic";
        }

        private void ShowMessage(string message, string cssClass)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = cssClass;
            lblMessage.Visible = true;
        }
    }
}
