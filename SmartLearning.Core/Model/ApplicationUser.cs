
namespace SmartLearning.Core.Model
{
    public class ApplicationUser : IdentityUser
    {

        public ICollection<Course> Courses { get; set; } = new List<Course>();
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<StudentAnswer> StudentAnswers { get; set; } = new List<StudentAnswer>();
        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
        public ICollection<Grades> Grades { get; set; } = new List<Grades>();
        // Instructor properties
        public string? JobTitle { get; set; }
        public int? NumberOfStudents { get; set; }
        public double? Rating { get; set; }
        public string? YoutubeChannelUrl { get; set; }

    }
}