
namespace SmartLearning.Application.DTOs
{
    public class CreateInstructorDto
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string JobTitle { get; set; }
        public int? NumberOfStudents { get; set; }
        public double? Rating { get; set; }
        public string PhoneNumber { get; set; }
        public string YoutubeChannelUrl { get; set; }
        public int? CustomNumberId { get; set; }
    }
}
