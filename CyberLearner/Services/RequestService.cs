using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using CyberLearner.Data;
using CyberLearner.Models;

namespace CyberLearner.Services
{
    public class RequestService
    {
        public int GetRetakeWaitDays()
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings["RetakeWaitDays"]);
        }

        public DirectExamRequest GetLatestDirectRequest(int userId)
        {
            string sql = "SELECT TOP 1 * FROM DirectExamRequests WHERE UserID = @UserID ORDER BY RequestDate DESC";
            DataTable table = DatabaseHelper.GetTable(sql, new SqlParameter("@UserID", userId));

            if (table.Rows.Count == 0)
            {
                return null;
            }

            DataRow row = table.Rows[0];
            DirectExamRequest request = new DirectExamRequest();
            request.RequestID = Convert.ToInt32(row["RequestID"]);
            request.UserID = Convert.ToInt32(row["UserID"]);
            request.RequestDate = Convert.ToDateTime(row["RequestDate"]);
            request.Status = row["Status"].ToString();

            if (row["ReviewedBy"] != DBNull.Value)
            {
                request.ReviewedBy = Convert.ToInt32(row["ReviewedBy"]);
                request.ReviewedDate = Convert.ToDateTime(row["ReviewedDate"]);
            }

            return request;
        }

        public void CreateDirectRequest(int userId)
        {
            string sql = "INSERT INTO DirectExamRequests (UserID, RequestDate, Status) VALUES (@UserID, GETDATE(), 'Pending')";
            DatabaseHelper.Execute(sql, new SqlParameter("@UserID", userId));
        }

        public DataTable GetDirectRequestsByUser(int userId)
        {
            string sql = "SELECT r.RequestID, r.RequestDate, r.Status, r.ReviewedDate, m.FullName AS ReviewerName " +
                         "FROM DirectExamRequests r LEFT JOIN Users m ON r.ReviewedBy = m.UserID " +
                         "WHERE r.UserID = @UserID ORDER BY r.RequestDate DESC";

            return DatabaseHelper.GetTable(sql, new SqlParameter("@UserID", userId));
        }

        public DataTable GetDirectRequests(string status)
        {
            string sql = "SELECT r.RequestID, u.FullName, u.Username, r.RequestDate, r.Status, r.ReviewedDate, m.FullName AS ReviewerName " +
                         "FROM DirectExamRequests r INNER JOIN Users u ON r.UserID = u.UserID " +
                         "LEFT JOIN Users m ON r.ReviewedBy = m.UserID " +
                         "WHERE (@Status = 'All' OR r.Status = @Status) ORDER BY r.RequestDate DESC";

            return DatabaseHelper.GetTable(sql, new SqlParameter("@Status", status));
        }

        public void ReviewDirectRequest(int requestId, string status, int reviewerId)
        {
            string sql = "UPDATE DirectExamRequests SET Status = @Status, ReviewedBy = @ReviewedBy, ReviewedDate = GETDATE() " +
                         "WHERE RequestID = @RequestID AND Status = 'Pending'";

            DatabaseHelper.Execute(sql,
                new SqlParameter("@Status", status),
                new SqlParameter("@ReviewedBy", reviewerId),
                new SqlParameter("@RequestID", requestId));
        }

        public RetakeRequest GetLatestRetakeRequest(int userId, int examAttemptId)
        {
            string sql = "SELECT TOP 1 * FROM RetakeRequests WHERE UserID = @UserID AND ExamAttemptID = @ExamAttemptID " +
                         "ORDER BY RequestDate DESC";

            DataTable table = DatabaseHelper.GetTable(sql,
                new SqlParameter("@UserID", userId),
                new SqlParameter("@ExamAttemptID", examAttemptId));

            if (table.Rows.Count == 0)
            {
                return null;
            }

            DataRow row = table.Rows[0];
            RetakeRequest request = new RetakeRequest();
            request.RetakeRequestID = Convert.ToInt32(row["RetakeRequestID"]);
            request.UserID = Convert.ToInt32(row["UserID"]);
            request.ExamAttemptID = Convert.ToInt32(row["ExamAttemptID"]);
            request.RequestDate = Convert.ToDateTime(row["RequestDate"]);
            request.Status = row["Status"].ToString();

            if (row["ReviewedBy"] != DBNull.Value)
            {
                request.ReviewedBy = Convert.ToInt32(row["ReviewedBy"]);
                request.ReviewedDate = Convert.ToDateTime(row["ReviewedDate"]);
            }

            return request;
        }

        public void CreateRetakeRequest(int userId, int examAttemptId)
        {
            string sql = "INSERT INTO RetakeRequests (UserID, ExamAttemptID, RequestDate, Status) " +
                         "VALUES (@UserID, @ExamAttemptID, GETDATE(), 'Pending')";

            DatabaseHelper.Execute(sql,
                new SqlParameter("@UserID", userId),
                new SqlParameter("@ExamAttemptID", examAttemptId));
        }

        public DataTable GetRetakeRequestsByUser(int userId)
        {
            string sql = "SELECT r.RetakeRequestID, a.AttemptNumber, a.Score, r.RequestDate, r.Status, r.ReviewedDate, m.FullName AS ReviewerName " +
                         "FROM RetakeRequests r INNER JOIN ExamAttempts a ON r.ExamAttemptID = a.ExamAttemptID " +
                         "LEFT JOIN Users m ON r.ReviewedBy = m.UserID " +
                         "WHERE r.UserID = @UserID ORDER BY r.RequestDate DESC";

            return DatabaseHelper.GetTable(sql, new SqlParameter("@UserID", userId));
        }

        public DataTable GetRetakeRequests(string status)
        {
            string sql = "SELECT r.RetakeRequestID, u.FullName, u.Username, a.AttemptNumber, a.Score, a.AttemptDate, " +
                         "r.RequestDate, r.Status, r.ReviewedDate, m.FullName AS ReviewerName " +
                         "FROM RetakeRequests r INNER JOIN Users u ON r.UserID = u.UserID " +
                         "INNER JOIN ExamAttempts a ON r.ExamAttemptID = a.ExamAttemptID " +
                         "LEFT JOIN Users m ON r.ReviewedBy = m.UserID " +
                         "WHERE (@Status = 'All' OR r.Status = @Status) ORDER BY r.RequestDate DESC";

            return DatabaseHelper.GetTable(sql, new SqlParameter("@Status", status));
        }

        public void ReviewRetakeRequest(int retakeRequestId, string status, int reviewerId)
        {
            string sql = "UPDATE RetakeRequests SET Status = @Status, ReviewedBy = @ReviewedBy, ReviewedDate = GETDATE() " +
                         "WHERE RetakeRequestID = @RetakeRequestID AND Status = 'Pending'";

            DatabaseHelper.Execute(sql,
                new SqlParameter("@Status", status),
                new SqlParameter("@ReviewedBy", reviewerId),
                new SqlParameter("@RetakeRequestID", retakeRequestId));
        }

        public int CountPendingRequests()
        {
            string sql = "SELECT (SELECT COUNT(*) FROM DirectExamRequests WHERE Status = 'Pending') + " +
                         "(SELECT COUNT(*) FROM RetakeRequests WHERE Status = 'Pending')";

            return Convert.ToInt32(DatabaseHelper.GetValue(sql));
        }
    }
}
