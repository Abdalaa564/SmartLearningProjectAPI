
namespace SmartLearning.Core.Model
{
    public class Course
    {
        [Key]
        public int Crs_Id { get; set; }

        [Required, MaxLength(100)]
        public string Crs_Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Crs_Description { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required]
        public string User_Id { get; set; } = string.Empty; // Foreign Key  Course  Instructor

        public ApplicationUser User { get; set; } = null!; // Course  Instructor (1 → 1)

        //(M → 1)
        public ICollection<Unit> Units { get; set; } = new List<Unit>();
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();  // Course  Students (1 → M)
        //public ICollection<Grades> Grades { get; set; } = new List<Grades>();

    }
}