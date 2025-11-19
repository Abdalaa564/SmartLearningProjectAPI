

namespace SmartLearning.Core.Model
{
    public class Lessons
    {
        public int Lesson_Id { get; set; }
        public int Unit_Id { get; set; }
        public string Lesson_Name { get; set; } = string.Empty;
        public string LessonDescription { get; set; } = string.Empty;

        public Unit Unit { get; set; } = null!;
        public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
        public ICollection<Resource> Resources { get; set; } = new List<Resource>();
        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    }
}
