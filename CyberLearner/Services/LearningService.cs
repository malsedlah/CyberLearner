using System;
using System.Data;
using System.Data.SqlClient;
using CyberLearner.Data;
using CyberLearner.Models;

namespace CyberLearner.Services
{
    public class LearningService
    {
        public DataTable GetActiveTopics()
        {
            string sql = "SELECT TopicID, TopicTitle, TopicOrder FROM Topics WHERE Status = 'Active' ORDER BY TopicOrder";
            return DatabaseHelper.GetTable(sql);
        }

        public DataTable GetAllTopics()
        {
            string sql = "SELECT TopicID, TopicTitle, TopicOrder, Status FROM Topics ORDER BY TopicOrder";
            return DatabaseHelper.GetTable(sql);
        }

        public Topic GetTopic(int topicId)
        {
            string sql = "SELECT * FROM Topics WHERE TopicID = @TopicID";
            DataTable table = DatabaseHelper.GetTable(sql, new SqlParameter("@TopicID", topicId));

            if (table.Rows.Count == 0)
            {
                return null;
            }

            DataRow row = table.Rows[0];
            Topic topic = new Topic();
            topic.TopicID = Convert.ToInt32(row["TopicID"]);
            topic.TopicTitle = row["TopicTitle"].ToString();
            topic.TopicContent = row["TopicContent"].ToString();
            topic.TopicOrder = Convert.ToInt32(row["TopicOrder"]);
            topic.Status = row["Status"].ToString();
            return topic;
        }

        public bool HasPassedTopic(int userId, int topicId)
        {
            string sql = "SELECT COUNT(*) FROM TopicQuizAttempts WHERE UserID = @UserID AND TopicID = @TopicID AND PassStatus = 'Pass'";
            int count = Convert.ToInt32(DatabaseHelper.GetValue(sql,
                new SqlParameter("@UserID", userId),
                new SqlParameter("@TopicID", topicId)));
            return count > 0;
        }

        public bool IsTopicUnlocked(int userId, int topicId)
        {
            DataTable topics = GetActiveTopics();
            bool previousPassed = true;

            foreach (DataRow row in topics.Rows)
            {
                int currentId = Convert.ToInt32(row["TopicID"]);

                if (currentId == topicId)
                {
                    return previousPassed;
                }

                previousPassed = HasPassedTopic(userId, currentId);
            }

            return false;
        }

        public int CountActiveTopics()
        {
            return GetActiveTopics().Rows.Count;
        }

        public int CountPassedTopics(int userId)
        {
            int passed = 0;

            foreach (DataRow row in GetActiveTopics().Rows)
            {
                if (HasPassedTopic(userId, Convert.ToInt32(row["TopicID"])))
                {
                    passed++;
                }
            }

            return passed;
        }

        public bool HasCompletedCourse(int userId)
        {
            int total = CountActiveTopics();
            return total > 0 && CountPassedTopics(userId) == total;
        }

        public int GetNextTopicId(int topicId)
        {
            DataTable topics = GetActiveTopics();

            for (int i = 0; i < topics.Rows.Count - 1; i++)
            {
                if (Convert.ToInt32(topics.Rows[i]["TopicID"]) == topicId)
                {
                    return Convert.ToInt32(topics.Rows[i + 1]["TopicID"]);
                }
            }

            return -1;
        }

        public bool TopicOrderExists(int topicOrder, int ignoreTopicId)
        {
            string sql = "SELECT COUNT(*) FROM Topics WHERE TopicOrder = @TopicOrder AND TopicID <> @TopicID";
            int count = Convert.ToInt32(DatabaseHelper.GetValue(sql,
                new SqlParameter("@TopicOrder", topicOrder),
                new SqlParameter("@TopicID", ignoreTopicId)));
            return count > 0;
        }

        public void AddTopic(Topic topic)
        {
            string sql = "INSERT INTO Topics (TopicTitle, TopicContent, TopicOrder, Status) " +
                         "VALUES (@TopicTitle, @TopicContent, @TopicOrder, @Status)";

            DatabaseHelper.Execute(sql,
                new SqlParameter("@TopicTitle", topic.TopicTitle),
                new SqlParameter("@TopicContent", topic.TopicContent),
                new SqlParameter("@TopicOrder", topic.TopicOrder),
                new SqlParameter("@Status", topic.Status));
        }

        public void UpdateTopic(Topic topic)
        {
            string sql = "UPDATE Topics SET TopicTitle = @TopicTitle, TopicContent = @TopicContent, " +
                         "TopicOrder = @TopicOrder, Status = @Status WHERE TopicID = @TopicID";

            DatabaseHelper.Execute(sql,
                new SqlParameter("@TopicTitle", topic.TopicTitle),
                new SqlParameter("@TopicContent", topic.TopicContent),
                new SqlParameter("@TopicOrder", topic.TopicOrder),
                new SqlParameter("@Status", topic.Status),
                new SqlParameter("@TopicID", topic.TopicID));
        }
    }
}
