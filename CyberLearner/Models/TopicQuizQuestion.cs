using System;

namespace CyberLearner.Models
{
    [Serializable]
    public class TopicQuizQuestion
    {
        public int QuestionID { get; set; }
        public int TopicID { get; set; }
        public string QuestionText { get; set; }
        public string OptionA { get; set; }
        public string OptionB { get; set; }
        public string OptionC { get; set; }
        public string OptionD { get; set; }
        public string CorrectAnswer { get; set; }
        public string Status { get; set; }
    }
}
