

namespace SmartLearning.Application.DTOs.InstructorDto
{
    public class RegisterInstructorDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required]
        public string FullName { get; set; } = string.Empty;

        public string? JobTitle { get; set; }

        public string? PhoneNumber { get; set; }

        public string? YoutubeChannelUrl { get; set; }

        public string? PhotoUrl { get; set; }

        public string? CertificateUrl { get; set; }

        public string? CvUrl { get; set; }        
        public string? Specialization { get; set; } 
        public string? UniversityName { get; set; } 
        public string? About { get; set; }         
    }
}
