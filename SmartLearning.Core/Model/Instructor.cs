

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

        [MaxLength(300)]
        public string? PhotoUrl { get; set; }

        [MaxLength(300)]
        public string? CertificateUrl { get; set; }
        [MaxLength(300)]
        public string? CvUrl { get; set; }          

        [MaxLength(200)]
        public string? Specialization { get; set; } 

        [MaxLength(200)]
        public string? UniversityName { get; set; }

        [MaxLength(1000)]
        public string? About { get; set; }          

        // 🆕 STATUS علشان approval من الـ Admin
        public InstructorStatus? Status { get; set; } = InstructorStatus.Pending;


        public ICollection<Course> Courses { get; set; } = new List<Course>();

		public ICollection<InstructorRating> InstructorRatings { get; set; } = new List<InstructorRating>();


	}
}
