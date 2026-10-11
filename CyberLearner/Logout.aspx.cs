using System;
using CyberLearner.Security;

namespace CyberLearner
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SessionHelper.SignOut();
            Response.Redirect("~/Login.aspx?loggedout=1");
        }
    }
}
