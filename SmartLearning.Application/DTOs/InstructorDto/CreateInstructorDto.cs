

namespace SmartLearning.Application.DTOs.InstructorDto
{
    public class CreateInstructorDto
    {
        [Required]
        public string UserId { get; set; } = string.Empty;   // Id بتاع ApplicationUser (Identity)

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? JobTitle { get; set; }


        public double? Rating { get; set; }

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [MaxLength(200)]
        public string? YoutubeChannelUrl { get; set; }
        [MaxLength(300)]
        public string? PhotoUrl { get; set; }

        [MaxLength(300)]
        public string? CertificateUrl { get; set; }
    }
}