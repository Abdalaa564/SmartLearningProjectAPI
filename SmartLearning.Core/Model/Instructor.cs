

namespace SmartLearning.Core.Model
{
    public class Instructor
    {
        [Key]
        public int Id { get; set; }

        // الربط مع ApplicationUser (اليوزر بتاع الـ Identity)
        [Required]
        [ForeignKey(nameof(User))]
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? JobTitle { get; set; }

        public int? NumberOfStudents { get; set; }

        public double? Rating { get; set; }

        [Phone]
        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [MaxLength(200)]
        public string? YoutubeChannelUrl { get; set; }

        public ICollection<Course> Courses { get; set; } = new List<Course>();


    }
}
