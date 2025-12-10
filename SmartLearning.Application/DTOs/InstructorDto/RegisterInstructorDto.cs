

namespace SmartLearning.Application.DTOs.InstructorDto
{
    public class RegisterInstructorDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirm password is required")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Full Name is required")]
        [MinLength(3, ErrorMessage = "Full Name must be at least 3 characters")]
        public string FullName { get; set; } = string.Empty;

        public string? JobTitle { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^01[0-9]{9}$", ErrorMessage = "Phone number must be Egyptian format (01XXXXXXXXX)")]
        public string? PhoneNumber { get; set; }

        [Url(ErrorMessage = "Invalid Youtube Channel URL")]
        public string? YoutubeChannelUrl { get; set; }
        [Url(ErrorMessage = "Invalid Photo URL")]
        public string? PhotoUrl { get; set; }

        public string? CertificateUrl { get; set; }

        [Url(ErrorMessage = "Invalid CV URL")]
        public string? CvUrl { get; set; }        
        public string? Specialization { get; set; } 
        public string? UniversityName { get; set; }

        [MaxLength(500, ErrorMessage = "About cannot exceed 500 characters")]
        public string? About { get; set; }         
    }
}
