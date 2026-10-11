using System;
using CyberLearner.Security;

namespace CyberLearner
{
    public partial class LearnerMaster : System.Web.UI.MasterPage
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            SessionHelper.RequireRole("Learner");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblUser.Text = Server.HtmlEncode(SessionHelper.GetFullName());
        }
    }
}
