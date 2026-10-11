using System;

namespace CyberLearner.Models
{
    public class TopicQuizAttempt
    {
        public int AttemptID { get; set; }
        public int UserID { get; set; }
        public int TopicID { get; set; }
        public decimal Score { get; set; }
        public string PassStatus { get; set; }
        public DateTime AttemptDate { get; set; }
    }
}
