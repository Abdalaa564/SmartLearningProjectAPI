

namespace SmartLearning.Application.DTOs.EnrollmentDto
{
    public class EnrollmentResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int? EnrollmentId { get; set; }
        public DateTime? EnrollmentDate { get; set; }
        public string? TransactionId { get; set; }
        public decimal? PaidAmount { get; set; }
    }
}
