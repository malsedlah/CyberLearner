using System;

namespace CyberLearner.Models
{
    public class RetakeRequest
    {
        public int RetakeRequestID { get; set; }
        public int UserID { get; set; }
        public int ExamAttemptID { get; set; }
        public DateTime RequestDate { get; set; }
        public string Status { get; set; }
        public int? ReviewedBy { get; set; }
        public DateTime? ReviewedDate { get; set; }
    }
}
