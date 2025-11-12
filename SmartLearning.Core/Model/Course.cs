namespace SmartLearning.Core.Model
{
    public class Course
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int Hours { get; set; }

        public ICollection<Student> Students { get; set; } = new List<Student>();

        public ICollection<InstructorCourse> InstructorCourses { get; set; } = new List<InstructorCourse>();

    }
}