namespace CyberLearner.Models
{
    public class ExamQuestion
    {
        public int ExamQuestionID { get; set; }
        public string QuestionText { get; set; }
        public string OptionA { get; set; }
        public string OptionB { get; set; }
        public string OptionC { get; set; }
        public string OptionD { get; set; }
        public string CorrectAnswer { get; set; }
        public string Status { get; set; }
    }
}
