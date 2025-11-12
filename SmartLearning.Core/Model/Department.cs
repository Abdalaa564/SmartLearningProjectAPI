
namespace SmartLearning.Core.Model
{
    public class Department
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [StringLength(10)]
        public string Code { get; set; }

        public string Address { get; set; }

        public string ManagerName { get; set; }

        public ICollection<Instructor> Instructors { get; set; } = new List<Instructor>();
        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
