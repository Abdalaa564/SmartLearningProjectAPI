
namespace SmartLearning.Core.Model
{
    public class Quiz
    {
        public int Quiz_Id { get; set; }
        public int Lesson_Id { get; set; }
        public string Quiz_Name { get; set; } = string.Empty;
        public int TotalMarks { get; set; }
        public int Duration { get; set; }

        public Lessons Lesson { get; set; } = null!;
        public ICollection<Questions> Questions { get; set; } = new List<Questions>();
        public ICollection<StudentAnswer> StudentAnswers { get; set; } = new List<StudentAnswer>();
        public ICollection<Grades> Grades { get; set; } = new List<Grades>();
    }
}