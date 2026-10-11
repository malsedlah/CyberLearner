using System;
using CyberLearner.Services;

namespace CyberLearner.Management
{
    public partial class LearnerResults : System.Web.UI.Page
    {
        ExamService examService = new ExamService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadResults();
            }
        }

        private void LoadResults()
        {
            string search = txtSearch.Text.Trim();
            string result = ddlResult.SelectedValue;

            gvExam.DataSource = examService.GetAllExamResults(search, result);
            gvExam.DataBind();

            gvQuiz.DataSource = examService.GetAllQuizResults(search, result);
            gvQuiz.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadResults();
        }

        protected void btnShowAll_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            ddlResult.SelectedValue = "All";
            LoadResults();
        }
    }
}
