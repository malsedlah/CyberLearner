using System;
using CyberLearner.Security;

namespace CyberLearner
{
    public partial class ManagementMaster : System.Web.UI.MasterPage
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            SessionHelper.RequireRole("Management");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblUser.Text = Server.HtmlEncode(SessionHelper.GetFullName());
        }
    }
}
