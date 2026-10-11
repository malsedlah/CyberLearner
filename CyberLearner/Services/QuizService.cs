using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using CyberLearner.Data;
using CyberLearner.Models;

namespace CyberLearner.Services
{
    public class QuizService
    {
        public int GetPassMark()
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings["QuizPassMark"]);
        }

        public List<TopicQuizQuestion> GetQuizQuestions(int topicId)
        {
            int count = Convert.ToInt32(ConfigurationManager.AppSettings["QuizQuestionCount"]);

            string sql = "SELECT TOP (@Count) * FROM TopicQuizQuestions " +
                         "WHERE TopicID = @TopicID AND Status = 'Active' ORDER BY NEWID()";

            DataTable table = DatabaseHelper.GetTable(sql,
                new SqlParameter("@Count", count),
                new SqlParameter("@TopicID", topicId));

            List<TopicQuizQuestion> questions = new List<TopicQuizQuestion>();

            foreach (DataRow row in table.Rows)
            {
                questions.Add(ReadQuestion(row));
            }

            return questions;
        }

        public int SaveAttempt(int userId, int topicId, decimal score, string passStatus)
        {
            string sql = "INSERT INTO TopicQuizAttempts (UserID, TopicID, Score, PassStatus, AttemptDate) " +
                         "VALUES (@UserID, @TopicID, @Score, @PassStatus, GETDATE()); SELECT SCOPE_IDENTITY();";

            object id = DatabaseHelper.GetValue(sql,
                new SqlParameter("@UserID", userId),
                new SqlParameter("@TopicID", topicId),
                new SqlParameter("@Score", score),
                new SqlParameter("@PassStatus", passStatus));

            return Convert.ToInt32(id);
        }

        public TopicQuizAttempt GetAttempt(int attemptId)
        {
            string sql = "SELECT * FROM TopicQuizAttempts WHERE AttemptID = @AttemptID";
            DataTable table = DatabaseHelper.GetTable(sql, new SqlParameter("@AttemptID", attemptId));

            if (table.Rows.Count == 0)
            {
                return null;
            }

            DataRow row = table.Rows[0];
            TopicQuizAttempt attempt = new TopicQuizAttempt();
            attempt.AttemptID = Convert.ToInt32(row["AttemptID"]);
            attempt.UserID = Convert.ToInt32(row["UserID"]);
            attempt.TopicID = Convert.ToInt32(row["TopicID"]);
            attempt.Score = Convert.ToDecimal(row["Score"]);
            attempt.PassStatus = row["PassStatus"].ToString();
            attempt.AttemptDate = Convert.ToDateTime(row["AttemptDate"]);
            return attempt;
        }

        public DataTable GetAttemptsByUser(int userId)
        {
            string sql = "SELECT a.AttemptID, t.TopicTitle, a.Score, a.PassStatus, a.AttemptDate " +
                         "FROM TopicQuizAttempts a INNER JOIN Topics t ON a.TopicID = t.TopicID " +
                         "WHERE a.UserID = @UserID ORDER BY a.AttemptDate DESC";

            return DatabaseHelper.GetTable(sql, new SqlParameter("@UserID", userId));
        }

        public DataTable GetQuestionsByTopic(int topicId)
        {
            string sql = "SELECT QuestionID, QuestionText, CorrectAnswer, Status FROM TopicQuizQuestions " +
                         "WHERE TopicID = @TopicID ORDER BY QuestionID";

            return DatabaseHelper.GetTable(sql, new SqlParameter("@TopicID", topicId));
        }

        public TopicQuizQuestion GetQuestion(int questionId)
        {
            string sql = "SELECT * FROM TopicQuizQuestions WHERE QuestionID = @QuestionID";
            DataTable table = DatabaseHelper.GetTable(sql, new SqlParameter("@QuestionID", questionId));

            if (table.Rows.Count == 0)
            {
                return null;
            }

            return ReadQuestion(table.Rows[0]);
        }

        public void AddQuestion(TopicQuizQuestion question)
        {
            string sql = "INSERT INTO TopicQuizQuestions (TopicID, QuestionText, OptionA, OptionB, OptionC, OptionD, CorrectAnswer, Status) " +
                         "VALUES (@TopicID, @QuestionText, @OptionA, @OptionB, @OptionC, @OptionD, @CorrectAnswer, @Status)";

            DatabaseHelper.Execute(sql,
                new SqlParameter("@TopicID", question.TopicID),
                new SqlParameter("@QuestionText", question.QuestionText),
                new SqlParameter("@OptionA", question.OptionA),
                new SqlParameter("@OptionB", question.OptionB),
                new SqlParameter("@OptionC", question.OptionC),
                new SqlParameter("@OptionD", question.OptionD),
                new SqlParameter("@CorrectAnswer", question.CorrectAnswer),
                new SqlParameter("@Status", question.Status));
        }

        public void UpdateQuestion(TopicQuizQuestion question)
        {
            string sql = "UPDATE TopicQuizQuestions SET TopicID = @TopicID, QuestionText = @QuestionText, OptionA = @OptionA, " +
                         "OptionB = @OptionB, OptionC = @OptionC, OptionD = @OptionD, CorrectAnswer = @CorrectAnswer, " +
                         "Status = @Status WHERE QuestionID = @QuestionID";

            DatabaseHelper.Execute(sql,
                new SqlParameter("@TopicID", question.TopicID),
                new SqlParameter("@QuestionText", question.QuestionText),
                new SqlParameter("@OptionA", question.OptionA),
                new SqlParameter("@OptionB", question.OptionB),
                new SqlParameter("@OptionC", question.OptionC),
                new SqlParameter("@OptionD", question.OptionD),
                new SqlParameter("@CorrectAnswer", question.CorrectAnswer),
                new SqlParameter("@Status", question.Status),
                new SqlParameter("@QuestionID", question.QuestionID));
        }

        public void DeleteQuestion(int questionId)
        {
            string sql = "DELETE FROM TopicQuizQuestions WHERE QuestionID = @QuestionID";
            DatabaseHelper.Execute(sql, new SqlParameter("@QuestionID", questionId));
        }

        private TopicQuizQuestion ReadQuestion(DataRow row)
        {
            TopicQuizQuestion question = new TopicQuizQuestion();
            question.QuestionID = Convert.ToInt32(row["QuestionID"]);
            question.TopicID = Convert.ToInt32(row["TopicID"]);
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
