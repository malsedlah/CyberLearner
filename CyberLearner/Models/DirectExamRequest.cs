using System;

namespace CyberLearner.Models
{
    public class DirectExamRequest
    {
        public int RequestID { get; set; }
        public int UserID { get; set; }
        public DateTime RequestDate { get; set; }
        public string Status { get; set; }
        public int? ReviewedBy { get; set; }
        public DateTime? ReviewedDate { get; set; }
    }
}
