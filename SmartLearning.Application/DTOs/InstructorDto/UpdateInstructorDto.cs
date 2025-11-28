namespace SmartLearning.Application.DTOs.Instructors
{
    public class UpdateInstructorDto
    {
        [MaxLength(100)]
        public string? FullName { get; set; }

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
