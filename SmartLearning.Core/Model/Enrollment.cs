
namespace SmartLearning.Core.Model
{
    public class Enrollment
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public double ProgressPercentage { get; set; }
    }
}
