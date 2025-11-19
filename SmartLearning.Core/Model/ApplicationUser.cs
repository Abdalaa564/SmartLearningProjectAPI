
namespace SmartLearning.Core.Model
{
    public class ApplicationUser : IdentityUser
    {

        public ICollection<Course> Courses { get; set; } = new List<Course>();
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<StudentAnswer> StudentAnswers { get; set; } = new List<StudentAnswer>();
        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    }
}