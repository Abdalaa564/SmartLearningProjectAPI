namespace SmartLearning.Application.DTOs.Instructors
{
    public class InstructorResponseDto
    {
        public int Id { get; set; }              // Id تبع جدول Instructor
        public string UserId { get; set; } = string.Empty;  // ApplicationUser.Id

        public string FullName { get; set; } = string.Empty;
        public string? JobTitle { get; set; }
        public int? NumberOfStudents { get; set; }
        public double? Rating { get; set; }
        public string? PhoneNumber { get; set; }
        public string? YoutubeChannelUrl { get; set; }

        // لو حابب تزيد:
        public string? Email { get; set; }       // من ApplicationUser
    }
}
