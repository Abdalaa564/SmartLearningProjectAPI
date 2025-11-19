
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartLearning.Core.Model
{
    public class Rating
    {
        [Key, Column(Order = 0)]
        public int Lesson_Id { get; set; }
        [Key, Column(Order = 1)]
        public string User_Id { get; set; } = string.Empty;
        public int RatingValue { get; set; }

        [MaxLength(1000)]
        public string Feedback { get; set; } = string.Empty;

        public Lessons Lesson { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
    }
}
