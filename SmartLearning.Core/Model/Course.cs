
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

        [MaxLength(300)]
        public string? ImageUrl { get; set; }

        [Required]
        public int InstructorId { get; set; }   // FK → Instructor
        public Instructor Instructor { get; set; } = null!;

        public ICollection<Unit> Units { get; set; } = new List<Unit>();

        // 👈 هنا العلاقة اللي مع Enrollment
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

        public ICollection<Grades> Grades { get; set; } = new List<Grades>();

		public ICollection<CourseRating> CourseRatings { get; set; } = new List<CourseRating>();

	}
}