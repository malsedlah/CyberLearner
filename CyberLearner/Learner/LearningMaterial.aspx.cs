using System;
using CyberLearner.Models;
using CyberLearner.Security;
using CyberLearner.Services;

namespace CyberLearner.Learner
{
    public partial class LearningMaterial : System.Web.UI.Page
    {
        LearningService learningService = new LearningService();

        protected void Page_Load(object sender, EventArgs e)
        {
            int topicId;

            if (!int.TryParse(Request.QueryString["topicId"], out topicId))
            {
                Response.Redirect("~/Learner/Topics.aspx");
            }

            int userId = SessionHelper.GetUserID();
            Topic topic = learningService.GetTopic(topicId);

            if (topic == null || topic.Status != "Active" || !learningService.IsTopicUnlocked(userId, topicId))
            {
                Response.Redirect("~/Learner/Topics.aspx");
            }

            Page.Title = topic.TopicTitle;
            lblTitle.Text = "Topic " + topic.TopicOrder + ": " + Server.HtmlEncode(topic.TopicTitle);
            litContent.Text = topic.TopicContent;
            lnkQuiz.NavigateUrl = "TopicQuiz.aspx?topicId=" + topicId;

            if (learningService.HasPassedTopic(userId, topicId))
            {
                lblStatus.Text = "You have already passed the quiz for this topic. You can take it again for practice.";
                lnkQuiz.Text = "Retake Topic Quiz";

                int nextTopicId = learningService.GetNextTopicId(topicId);

                if (nextTopicId != -1)
                {
                    lnkNext.NavigateUrl = "LearningMaterial.aspx?topicId=" + nextTopicId;
                    lnkNext.Visible = true;
                }
            }
            else
            {
                lblStatus.Text = "Pass the quiz for this topic to unlock the next topic.";
            }
        }
    }
}
