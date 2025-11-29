

using SmartLearning.Application.DTOs.PaymentDto;

namespace SmartLearning.Application.DTOs.EnrollmentDto
{
    public class EnrollmentRequestDto
    {
        [Required]
        public string StudentId { get; set; } = string.Empty;
        [Required]
        public int CourseId { get; set; }

        [Required]
        public PaymentInfoDto Payment { get; set; } = new PaymentInfoDto();


    }
}
