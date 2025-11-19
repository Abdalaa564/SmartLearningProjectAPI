
namespace SmartLearning.Core.Model
{
    public class Questions
    {
        [Key]
        public int Question_Id { get; set; }

        [Required]
        public int Quiz_Id { get; set; }

        [Required, MaxLength(500)]
        public string Question_Text { get; set; } = string.Empty;
        public QuestionType Question_Type { get; set; } 
        public int Grade_Point { get; set; }

        [MaxLength(500)]
        public string CorrectAnswer { get; set; } = string.Empty;

        public Quiz Quiz { get; set; } = null!;
        public ICollection<Choice> Choices { get; set; } = new List<Choice>();
        public ICollection<StudentAnswer> StudentAnswers { get; set; } = new List<StudentAnswer>();
    }
}