using System;
using System.Data;
using System.Data.SqlClient;
using CyberLearner.Data;
using CyberLearner.Models;
using CyberLearner.Security;

namespace CyberLearner.Services
{
    public class AuthService
    {
        public bool UsernameExists(string username)
        {
            string sql = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
            int count = Convert.ToInt32(DatabaseHelper.GetValue(sql, new SqlParameter("@Username", username)));
            return count > 0;
        }

        public bool EmailExists(string email)
        {
            string sql = "SELECT COUNT(*) FROM Users WHERE Email = @Email";
            int count = Convert.ToInt32(DatabaseHelper.GetValue(sql, new SqlParameter("@Email", email)));
            return count > 0;
        }

        public void RegisterLearner(string fullName, string email, string username, string password)
        {
            string sql = "INSERT INTO Users (FullName, Email, Username, PasswordHash, Role, AccountStatus) " +
                         "VALUES (@FullName, @Email, @Username, @PasswordHash, 'Learner', 'Active')";

            DatabaseHelper.Execute(sql,
                new SqlParameter("@FullName", fullName),
                new SqlParameter("@Email", email),
                new SqlParameter("@Username", username),
                new SqlParameter("@PasswordHash", PasswordHelper.HashPassword(password)));
        }

        public void CreateLearner(string fullName, string email, string username, string passwordHash)
        {
            string sql = "INSERT INTO Users (FullName, Email, Username, PasswordHash, Role, AccountStatus) " +
                         "VALUES (@FullName, @Email, @Username, @PasswordHash, 'Learner', 'Active')";

            DatabaseHelper.Execute(sql,
                new SqlParameter("@FullName", fullName),
                new SqlParameter("@Email", email),
                new SqlParameter("@Username", username),
                new SqlParameter("@PasswordHash", passwordHash));
        }

        public User Login(string username, string password)
        {
            string sql = "SELECT * FROM Users WHERE Username = @Username OR Email = @Username";
            DataTable table = DatabaseHelper.GetTable(sql, new SqlParameter("@Username", username));

            if (table.Rows.Count == 0)
            {
                return null;
            }

            User user = ReadUser(table.Rows[0]);

            if (!PasswordHelper.VerifyPassword(password, user.PasswordHash))
            {
                return null;
            }

            return user;
        }

        public User GetUserByEmail(string email)
        {
            string sql = "SELECT * FROM Users WHERE Email = @Email";
            DataTable table = DatabaseHelper.GetTable(sql, new SqlParameter("@Email", email));

            if (table.Rows.Count == 0)
            {
                return null;
            }

            return ReadUser(table.Rows[0]);
        }

        public void UpdatePassword(int userId, string newPassword)
        {
            string sql = "UPDATE Users SET PasswordHash = @PasswordHash WHERE UserID = @UserID";

            DatabaseHelper.Execute(sql,
                new SqlParameter("@PasswordHash", PasswordHelper.HashPassword(newPassword)),
                new SqlParameter("@UserID", userId));
        }

        public static User ReadUser(DataRow row)
        {
            User user = new User();
            user.UserID = Convert.ToInt32(row["UserID"]);
            user.FullName = row["FullName"].ToString();
            user.Email = row["Email"].ToString();
            user.Username = row["Username"].ToString();
            user.PasswordHash = row["PasswordHash"].ToString();
            user.Role = row["Role"].ToString();
            user.AccountStatus = row["AccountStatus"].ToString();
            user.CreatedAt = Convert.ToDateTime(row["CreatedAt"]);
            return user;
        }
    }
}
