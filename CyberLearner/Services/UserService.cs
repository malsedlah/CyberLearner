using System;
using System.Data;
using System.Data.SqlClient;
using CyberLearner.Data;
using CyberLearner.Models;

namespace CyberLearner.Services
{
    public class UserService
    {
        public User GetUserById(int userId)
        {
            string sql = "SELECT * FROM Users WHERE UserID = @UserID";
            DataTable table = DatabaseHelper.GetTable(sql, new SqlParameter("@UserID", userId));

            if (table.Rows.Count == 0)
            {
                return null;
            }

            return AuthService.ReadUser(table.Rows[0]);
        }

        public bool EmailUsedByOther(string email, int userId)
        {
            string sql = "SELECT COUNT(*) FROM Users WHERE Email = @Email AND UserID <> @UserID";
            int count = Convert.ToInt32(DatabaseHelper.GetValue(sql,
                new SqlParameter("@Email", email),
                new SqlParameter("@UserID", userId)));
            return count > 0;
        }

        public void UpdateProfile(int userId, string fullName, string email)
        {
            string sql = "UPDATE Users SET FullName = @FullName, Email = @Email WHERE UserID = @UserID";

            DatabaseHelper.Execute(sql,
                new SqlParameter("@FullName", fullName),
                new SqlParameter("@Email", email),
                new SqlParameter("@UserID", userId));
        }

        public DataTable GetLearners(string search)
        {
            string sql = "SELECT UserID, FullName, Email, Username, AccountStatus, CreatedAt FROM Users " +
                         "WHERE Role = 'Learner' AND (FullName LIKE @Search OR Email LIKE @Search OR Username LIKE @Search) " +
                         "ORDER BY CreatedAt DESC";

            return DatabaseHelper.GetTable(sql, new SqlParameter("@Search", "%" + search + "%"));
        }

        public void SetAccountStatus(int userId, string status)
        {
            string sql = "UPDATE Users SET AccountStatus = @Status WHERE UserID = @UserID AND Role = 'Learner'";

            DatabaseHelper.Execute(sql,
                new SqlParameter("@Status", status),
                new SqlParameter("@UserID", userId));
        }
    }
}
