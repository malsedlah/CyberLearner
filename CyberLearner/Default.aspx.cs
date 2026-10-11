using System;
using System.Data;
using CyberLearner.Data;

namespace CyberLearner
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable topics = DatabaseHelper.GetTable("SELECT TopicTitle FROM Topics WHERE Status = 'Active' ORDER BY TopicOrder");
                rptTopics.DataSource = topics;
                rptTopics.DataBind();

                rptTopics.Visible = topics.Rows.Count > 0;
                lblNoTopics.Visible = topics.Rows.Count == 0;
            }
        }
    }
}
