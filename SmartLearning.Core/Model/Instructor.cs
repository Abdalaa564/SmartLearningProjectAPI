
namespace SmartLearning.Core.Model
{
    public class Instructor
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        public string Salary { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public ICollection<InstructorCourse> InstructorCourses { get; set; } = new List<InstructorCourse>();


    }
}
