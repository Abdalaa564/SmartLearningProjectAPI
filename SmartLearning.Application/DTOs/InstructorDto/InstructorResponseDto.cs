namespace SmartLearning.Application.DTOs.Instructors
{
    public class InstructorResponseDto
    {
        public int Id { get; set; }              
        public string UserId { get; set; } = string.Empty;  

        public string FullName { get; set; } = string.Empty;
        public string? JobTitle { get; set; }
        public int? NumberOfStudents { get; set; }
        public double? Rating { get; set; }
        public string? PhoneNumber { get; set; }
        public string? YoutubeChannelUrl { get; set; }

        
        public string? Email { get; set; }

        public string? PhotoUrl { get; set; }
        public string? CertificateUrl { get; set; }
    }
}
