
namespace SmartLearning.Core.Model
{
    public class Enrollment
    {
        [Key]
        public int Enroll_Id { get; set; }

        // FK → Student
        [Required]
        [ForeignKey(nameof(Student))]
        public int StudentId { get; set; }
        public Student Student { get; set; } = null!;

        // FK → Course
        [Required]
        [ForeignKey(nameof(Course))]
        public int Crs_Id { get; set; }
        public Course Course { get; set; } = null!;   

        public DateTime Enroll_Date { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Paid_Amount { get; set; }


    //    public ApplicationUser User { get; set; } = null!;
       // public Course Course { get; set; } = new Course();

        public ICollection<Payment> Payments { get; set; } = new List<Payment>();

    }
}
    
