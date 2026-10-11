using System;
using System.Data;
using System.Data.SqlClient;
using CyberLearner.Data;
using CyberLearner.Models;

namespace CyberLearner.Services
{
    public class CertificateService
    {
        ExamService examService = new ExamService();

        public Certificate GetCertificateByUser(int userId)
        {
            string sql = "SELECT TOP 1 * FROM Certificates WHERE UserID = @UserID ORDER BY IssueDate";
            DataTable table = DatabaseHelper.GetTable(sql, new SqlParameter("@UserID", userId));

            if (table.Rows.Count == 0)
            {
                return null;
            }

            DataRow row = table.Rows[0];
            Certificate certificate = new Certificate();
            certificate.CertificateID = Convert.ToInt32(row["CertificateID"]);
            certificate.UserID = Convert.ToInt32(row["UserID"]);
            certificate.ExamAttemptID = Convert.ToInt32(row["ExamAttemptID"]);
            certificate.CertificateNumber = row["CertificateNumber"].ToString();
            certificate.IssueDate = Convert.ToDateTime(row["IssueDate"]);
            return certificate;
        }

        public Certificate IssueCertificate(int userId)
        {
            Certificate existing = GetCertificateByUser(userId);

            if (existing != null)
            {
                return existing;
            }

            ExamAttempt passedAttempt = examService.GetPassedAttempt(userId);

            if (passedAttempt == null)
            {
                return null;
            }

            string certificateNumber = "CF-" + DateTime.Now.Year + "-" + passedAttempt.ExamAttemptID.ToString("D4");

            string sql = "INSERT INTO Certificates (UserID, ExamAttemptID, CertificateNumber, IssueDate) " +
                         "VALUES (@UserID, @ExamAttemptID, @CertificateNumber, GETDATE())";

            DatabaseHelper.Execute(sql,
                new SqlParameter("@UserID", userId),
                new SqlParameter("@ExamAttemptID", passedAttempt.ExamAttemptID),
                new SqlParameter("@CertificateNumber", certificateNumber));

            return GetCertificateByUser(userId);
        }

        public DataTable GetAllCertificates(string search)
        {
            string sql = "SELECT c.CertificateID, c.CertificateNumber, u.FullName, u.Email, a.AttemptNumber, a.Score, c.IssueDate " +
                         "FROM Certificates c INNER JOIN Users u ON c.UserID = u.UserID " +
                         "INNER JOIN ExamAttempts a ON c.ExamAttemptID = a.ExamAttemptID " +
                         "WHERE u.FullName LIKE @Search OR u.Email LIKE @Search OR c.CertificateNumber LIKE @Search " +
                         "ORDER BY c.IssueDate DESC";

            return DatabaseHelper.GetTable(sql, new SqlParameter("@Search", "%" + search + "%"));
        }

        public int CountCertificates()
        {
            return Convert.ToInt32(DatabaseHelper.GetValue("SELECT COUNT(*) FROM Certificates"));
        }
    }
}
