

namespace SmartLearning.Application.DTOs.InstructorDto
{
    public class CreateInstructorDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string FullName { get; set; } = string.Empty;

        public string? JobTitle { get; set; }


        public double? Rating { get; set; }
        public string? PhoneNumber { get; set; }
        public string? YoutubeChannelUrl { get; set; }
        public string? PhotoUrl { get; set; }
        public string? CertificateUrl { get; set; }
    }
}