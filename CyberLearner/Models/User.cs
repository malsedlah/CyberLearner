using System;

namespace CyberLearner.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
        public string AccountStatus { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
