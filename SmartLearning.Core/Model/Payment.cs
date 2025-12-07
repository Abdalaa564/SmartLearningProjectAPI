

namespace SmartLearning.Core.Model
{
    public class Payment
    {
        [Key]
        public int Payment_Id { get; set; }

        [Required]
        [ForeignKey(nameof(Enrollment))]
        public int Enroll_Id { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        [MaxLength(50)]
        public string Payment_Method { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Transaction_Id { get; set; } = string.Empty;

        [Required]
        public DateTime Payment_Date { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Pending";

        [Column(TypeName = "NVARCHAR(MAX)")]
        public string? Gateway_Response { get; set; }

        // Navigation property
        public Enrollment Enrollment { get; set; } = null!;
    }
}
