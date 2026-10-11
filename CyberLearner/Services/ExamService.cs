using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using CyberLearner.Data;
using CyberLearner.Models;

namespace CyberLearner.Services
{
    public class ExamService
    {
        LearningService learningService = new LearningService();

        public int GetPassMark()
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings["ExamPassMark"]);
        }

        public int GetQuestionCount()
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings["ExamQuestionCount"]);
        }

        public int CountAttempts(int userId)
        {
            string sql = "SELECT COUNT(*) FROM ExamAttempts WHERE UserID = @UserID";
            return Convert.ToInt32(DatabaseHelper.GetValue(sql, new SqlParameter("@UserID", userId)));
        }

        public ExamAttempt GetLatestAttempt(int userId)
        {
            string sql = "SELECT TOP 1 * FROM ExamAttempts WHERE UserID = @UserID ORDER BY AttemptNumber DESC";
            DataTable table = DatabaseHelper.GetTable(sql, new SqlParameter("@UserID", userId));

            if (table.Rows.Count == 0)
            {
                return null;
            }

            return ReadAttempt(table.Rows[0]);
        }

        public ExamAttempt GetPassedAttempt(int userId)
        {
            string sql = "SELECT TOP 1 * FROM ExamAttempts WHERE UserID = @UserID AND PassStatus = 'Pass' ORDER BY AttemptNumber";
            DataTable table = DatabaseHelper.GetTable(sql, new SqlParameter("@UserID", userId));

            if (table.Rows.Count == 0)
            {
                return null;
            }

            return ReadAttempt(table.Rows[0]);
        }

        public ExamAttempt GetAttempt(int examAttemptId)
        {
            string sql = "SELECT * FROM ExamAttempts WHERE ExamAttemptID = @ExamAttemptID";
            DataTable table = DatabaseHelper.GetTable(sql, new SqlParameter("@ExamAttemptID", examAttemptId));

            if (table.Rows.Count == 0)
            {
                return null;
            }

            return ReadAttempt(table.Rows[0]);
        }

        public bool HasApprovedDirectAccess(int userId)
        {
            string sql = "SELECT COUNT(*) FROM DirectExamRequests WHERE UserID = @UserID AND Status = 'Approved'";
            int count = Convert.ToInt32(DatabaseHelper.GetValue(sql, new SqlParameter("@UserID", userId)));
            return count > 0;
        }

        public bool HasApprovedRetake(int userId, int examAttemptId)
        {
            string sql = "SELECT COUNT(*) FROM RetakeRequests WHERE UserID = @UserID AND ExamAttemptID = @ExamAttemptID AND Status = 'Approved'";
            int count = Convert.ToInt32(DatabaseHelper.GetValue(sql,
                new SqlParameter("@UserID", userId),
                new SqlParameter("@ExamAttemptID", examAttemptId)));
            return count > 0;
        }

        public string GetAccessStatus(int userId)
        {
            if (GetPassedAttempt(userId) != null)
            {
                return "Passed";
            }

            ExamAttempt latest = GetLatestAttempt(userId);

            if (latest == null)
            {
                if (learningService.HasCompletedCourse(userId))
                {
                    return "EligibleCourse";
                }

                if (HasApprovedDirectAccess(userId))
                {
                    return "EligibleDirect";
                }

                return "NotEligible";
            }

            if (HasApprovedRetake(userId, latest.ExamAttemptID))
            {
                return "EligibleRetake";
            }

            return "RetakeRequired";
        }

        public bool CanTakeExam(int userId)
        {
            string status = GetAccessStatus(userId);
            return status == "EligibleCourse" || status == "EligibleDirect" || status == "EligibleRetake";
        }

        public List<ExamQuestion> GetExamQuestions()
        {
            string sql = "SELECT TOP (@Count) * FROM ExamQuestions WHERE Status = 'Active' ORDER BY NEWID()";
            DataTable table = DatabaseHelper.GetTable(sql, new SqlParameter("@Count", GetQuestionCount()));

            List<ExamQuestion> questions = new List<ExamQuestion>();

            foreach (DataRow row in table.Rows)
            {
                questions.Add(ReadQuestion(row));
            }

            return questions;
        }

        public int SaveAttempt(int userId, decimal score, string passStatus)
        {
            int attemptNumber = CountAttempts(userId) + 1;

            string sql = "INSERT INTO ExamAttempts (UserID, AttemptNumber, Score, PassStatus, AttemptDate) " +
                         "VALUES (@UserID, @AttemptNumber, @Score, @PassStatus, GETDATE()); SELECT SCOPE_IDENTITY();";

            object id = DatabaseHelper.GetValue(sql,
                new SqlParameter("@UserID", userId),
                new SqlParameter("@AttemptNumber", attemptNumber),
                new SqlParameter("@Score", score),
                new SqlParameter("@PassStatus", passStatus));

            return Convert.ToInt32(id);
        }

        public DataTable GetAttemptsByUser(int userId)
        {
            string sql = "SELECT ExamAttemptID, AttemptNumber, Score, PassStatus, AttemptDate FROM ExamAttempts " +
                         "WHERE UserID = @UserID ORDER BY AttemptNumber DESC";

            return DatabaseHelper.GetTable(sql, new SqlParameter("@UserID", userId));
        }

        public DataTable GetAllExamResults(string search, string passStatus)
        {
            string sql = "SELECT u.FullName, u.Username, a.AttemptNumber, a.Score, a.PassStatus, a.AttemptDate " +
                         "FROM ExamAttempts a INNER JOIN Users u ON a.UserID = u.UserID " +
                         "WHERE (u.FullName LIKE @Search OR u.Username LIKE @Search) " +
                         "AND (@PassStatus = 'All' OR a.PassStatus = @PassStatus) " +
                         "ORDER BY a.AttemptDate DESC";

            return DatabaseHelper.GetTable(sql,
                new SqlParameter("@Search", "%" + search + "%"),
                new SqlParameter("@PassStatus", passStatus));
        }

        public DataTable GetAllQuizResults(string search, string passStatus)
        {
            string sql = "SELECT u.FullName, u.Username, t.TopicTitle, a.Score, a.PassStatus, a.AttemptDate " +
                         "FROM TopicQuizAttempts a INNER JOIN Users u ON a.UserID = u.UserID " +
                         "INNER JOIN Topics t ON a.TopicID = t.TopicID " +
                         "WHERE (u.FullName LIKE @Search OR u.Username LIKE @Search) " +
                         "AND (@PassStatus = 'All' OR a.PassStatus = @PassStatus) " +
                         "ORDER BY a.AttemptDate DESC";

            return DatabaseHelper.GetTable(sql,
                new SqlParameter("@Search", "%" + search + "%"),
                new SqlParameter("@PassStatus", passStatus));
        }

        public DataTable GetAllQuestions()
        {
            string sql = "SELECT ExamQuestionID, QuestionText, CorrectAnswer, Status FROM ExamQuestions ORDER BY ExamQuestionID";
            return DatabaseHelper.GetTable(sql);
        }

        public int CountActiveQuestions()
        {
            string sql = "SELECT COUNT(*) FROM ExamQuestions WHERE Status = 'Active'";
            return Convert.ToInt32(DatabaseHelper.GetValue(sql));
        }

        public ExamQuestion GetQuestion(int examQuestionId)
        {
            string sql = "SELECT * FROM ExamQuestions WHERE ExamQuestionID = @ExamQuestionID";
            DataTable table = DatabaseHelper.GetTable(sql, new SqlParameter("@ExamQuestionID", examQuestionId));

            if (table.Rows.Count == 0)
            {
                return null;
            }

            return ReadQuestion(table.Rows[0]);
        }

        public void AddQuestion(ExamQuestion question)
        {
            string sql = "INSERT INTO ExamQuestions (QuestionText, OptionA, OptionB, OptionC, OptionD, CorrectAnswer, Status) " +
                         "VALUES (@QuestionText, @OptionA, @OptionB, @OptionC, @OptionD, @CorrectAnswer, @Status)";

            DatabaseHelper.Execute(sql,
                new SqlParameter("@QuestionText", question.QuestionText),
                new SqlParameter("@OptionA", question.OptionA),
                new SqlParameter("@OptionB", question.OptionB),
                new SqlParameter("@OptionC", question.OptionC),
                new SqlParameter("@OptionD", question.OptionD),
                new SqlParameter("@CorrectAnswer", question.CorrectAnswer),
                new SqlParameter("@Status", question.Status));
        }

        public void UpdateQuestion(ExamQuestion question)
        {
            string sql = "UPDATE ExamQuestions SET QuestionText = @QuestionText, OptionA = @OptionA, OptionB = @OptionB, " +
                         "OptionC = @OptionC, OptionD = @OptionD, CorrectAnswer = @CorrectAnswer, Status = @Status " +
                         "WHERE ExamQuestionID = @ExamQuestionID";

            DatabaseHelper.Execute(sql,
                new SqlParameter("@QuestionText", question.QuestionText),
                new SqlParameter("@OptionA", question.OptionA),
                new SqlParameter("@OptionB", question.OptionB),
                new SqlParameter("@OptionC", question.OptionC),
                new SqlParameter("@OptionD", question.OptionD),
                new SqlParameter("@CorrectAnswer", question.CorrectAnswer),
                new SqlParameter("@Status", question.Status),
                new SqlParameter("@ExamQuestionID", question.ExamQuestionID));
        }

        public void DeleteQuestion(int examQuestionId)
        {
            string sql = "DELETE FROM ExamQuestions WHERE ExamQuestionID = @ExamQuestionID";
            DatabaseHelper.Execute(sql, new SqlParameter("@ExamQuestionID", examQuestionId));
        }

        private ExamAttempt ReadAttempt(DataRow row)
        {
            ExamAttempt attempt = new ExamAttempt();
            attempt.ExamAttemptID = Convert.ToInt32(row["ExamAttemptID"]);
            attempt.UserID = Convert.ToInt32(row["UserID"]);
            attempt.AttemptNumber = Convert.ToInt32(row["AttemptNumber"]);
            attempt.Score = Convert.ToDecimal(row["Score"]);
            attempt.PassStatus = row["PassStatus"].ToString();
            attempt.AttemptDate = Convert.ToDateTime(row["AttemptDate"]);
            return attempt;
        }

        private ExamQuestion ReadQuestion(DataRow row)
        {
            ExamQuestion question = new ExamQuestion();
            question.ExamQuestionID = Convert.ToInt32(row["ExamQuestionID"]);
            question.QuestionText = row["QuestionText"].ToString();
            question.OptionA = row["OptionA"].ToString();
            question.OptionB = row["OptionB"].ToString();
            question.OptionC = row["OptionC"].ToString();
            question.OptionD = row["OptionD"].ToString();
            question.CorrectAnswer = row["CorrectAnswer"].ToString();
            question.Status = row["Status"].ToString();
            return question;
        }
    }
}
