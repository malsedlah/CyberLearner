using System;
using System.Web;
using CyberLearner.Models;

namespace CyberLearner.Security
{
    public static class SessionHelper
    {
        public static void SignIn(User user)
        {
            HttpContext.Current.Session["UserID"] = user.UserID;
            HttpContext.Current.Session["Role"] = user.Role;
            HttpContext.Current.Session["FullName"] = user.FullName;
        }

        public static void SignOut()
        {
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();
        }

        public static bool IsLoggedIn()
        {
            return HttpContext.Current.Session["UserID"] != null;
        }

        public static int GetUserID()
        {
            return Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        }

        public static string GetRole()
        {
            return Convert.ToString(HttpContext.Current.Session["Role"]);
        }

        public static string GetFullName()
        {
            return Convert.ToString(HttpContext.Current.Session["FullName"]);
        }

        public static void RedirectToDashboard()
        {
            if (GetRole() == "Management")
            {
                HttpContext.Current.Response.Redirect("~/Management/Dashboard.aspx");
            }
            else
            {
                HttpContext.Current.Response.Redirect("~/Learner/Dashboard.aspx");
            }
        }

        public static void RequireRole(string role)
        {
            if (!IsLoggedIn())
            {
                HttpContext.Current.Response.Redirect("~/Login.aspx");
            }

            if (GetRole() != role)
            {
                RedirectToDashboard();
            }
        }
    }
}
