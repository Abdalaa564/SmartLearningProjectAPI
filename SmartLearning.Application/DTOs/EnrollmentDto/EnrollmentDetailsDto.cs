

namespace SmartLearning.Application.DTOs.EnrollmentDto
{
    public class EnrollmentDetailsDto
    {
        public int EnrollId { get; set; }
        public int UserId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string StudentEmail { get; set; } = string.Empty;
        public string? StudentPhone { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public decimal CoursePrice { get; set; }
        public DateTime EnrollDate { get; set; }
        public decimal PaidAmount { get; set; }
        public string? PaymentStatus { get; set; }
        public string? TransactionId { get; set; }
        public DateTime? PaymentDate { get; set; }
    }
}
