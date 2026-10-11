using System;
using System.Data;
using CyberLearner.Security;
using CyberLearner.Services;

namespace CyberLearner.Learner
{
    public partial class Topics : System.Web.UI.Page
    {
        LearningService learningService = new LearningService();
        QuizService quizService = new QuizService();

        protected int ProgressPercent = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadTopics();
            }
        }

        private void LoadTopics()
        {
            int userId = SessionHelper.GetUserID();
            DataTable topics = learningService.GetActiveTopics();
            topics.Columns.Add("State");

            int passedCount = 0;
            bool previousPassed = true;

            foreach (DataRow row in topics.Rows)
            {
                bool passed = learningService.HasPassedTopic(userId, Convert.ToInt32(row["TopicID"]));

                if (passed)
                {
                    row["State"] = "Completed";
                    passedCount++;
                }
                else if (previousPassed)
                {
                    row["State"] = "Available";
                }
                else
                {
                    row["State"] = "Locked";
                }

                previousPassed = passed;
            }

            rptTopics.DataSource = topics;
            rptTopics.DataBind();

            int total = topics.Rows.Count;
            lblPassMark.Text = quizService.GetPassMark().ToString();
            lblProgress.Text = passedCount + " of " + total + " topics completed";

            if (total > 0)
            {
                ProgressPercent = passedCount * 100 / total;
            }

            lblNoTopics.Visible = total == 0;
            pnlCompleted.Visible = total > 0 && passedCount == total;
        }
    }
}
