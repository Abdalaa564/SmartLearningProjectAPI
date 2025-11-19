namespace SmartLearning.Core.Model
{
    public class Course
    {
        public int Crs_Id { get; set; }
        public string Crs_Name { get; set; } = string.Empty;
        public string Crs_Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string User_Id { get; set; } = string.Empty; // Foreign Key  Course  Instructor

        public ApplicationUser User { get; set; } = null!; // Course  Instructor (1 → 1)

        //(M → 1)
        public ICollection<Unit> Units { get; set; } = new List<Unit>();
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();  // Course  Students (1 → M)
        //public ICollection<Grades> Grades { get; set; } = new List<Grades>();

    }
}