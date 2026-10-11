using System;
using CyberLearner.Security;
using CyberLearner.Services;

namespace CyberLearner.Learner
{
    public partial class Results : System.Web.UI.Page
    {
        ExamService examService = new ExamService();
        QuizService quizService = new QuizService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int userId = SessionHelper.GetUserID();

                gvExam.DataSource = examService.GetAttemptsByUser(userId);
                gvExam.DataBind();

                gvQuiz.DataSource = quizService.GetAttemptsByUser(userId);
                gvQuiz.DataBind();
            }
        }
    }
}
