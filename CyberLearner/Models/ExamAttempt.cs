using System;

namespace CyberLearner.Models
{
    public class ExamAttempt
    {
        public int ExamAttemptID { get; set; }
        public int UserID { get; set; }
        public int AttemptNumber { get; set; }
        public decimal Score { get; set; }
        public string PassStatus { get; set; }
        public DateTime AttemptDate { get; set; }
    }
}
