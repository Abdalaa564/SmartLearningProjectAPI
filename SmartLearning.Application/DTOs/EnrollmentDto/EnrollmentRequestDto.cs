

using SmartLearning.Application.DTOs.PaymentDto;

namespace SmartLearning.Application.DTOs.EnrollmentDto
{
    public class EnrollmentRequestDto
    {
        [Required]
        public int StudentId { get; set; } 
        [Required]
        public int CourseId { get; set; }

        [Required]
        public PaymentInfoDto Payment { get; set; } = new PaymentInfoDto();

        //public string? TransactionId { get; set; }



    }
}
