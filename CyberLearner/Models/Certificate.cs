using System;

namespace CyberLearner.Models
{
    public class Certificate
    {
        public int CertificateID { get; set; }
        public int UserID { get; set; }
        public int ExamAttemptID { get; set; }
        public string CertificateNumber { get; set; }
        public DateTime IssueDate { get; set; }
    }
}
