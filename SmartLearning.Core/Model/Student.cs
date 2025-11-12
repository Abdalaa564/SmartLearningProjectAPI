namespace SmartLearning.Core.Model
{
    public class Student
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Address { get; set; }
        public string Email { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}