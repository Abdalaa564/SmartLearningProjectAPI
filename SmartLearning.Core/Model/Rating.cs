
namespace SmartLearning.Core.Model
{
    public class Rating
    {
        public int Lesson_Id { get; set; }
        public string User_Id { get; set; } = string.Empty;
        public int RatingValue { get; set; }
        public string Feedback { get; set; } = string.Empty;

        public Lessons Lesson { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
    }
}
