
namespace SmartLearning.Core.Model
{
    public class Enrollment
    {
        [Key]
        public int Enroll_Id { get; set; }

        [Required]
        public string User_Id { get; set; } = string.Empty;

        [Required]
        public int Crs_Id { get; set; }
        public DateTime Enroll_Date { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Paid_Amount { get; set; }

        public ApplicationUser User { get; set; } = null!;
        public Course Course { get; set; } = new Course();
    }
}
