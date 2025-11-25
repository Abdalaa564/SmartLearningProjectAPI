

namespace SmartLearning.Application.DTOs.Resource
{
    public class CreateYoutubeResourceDto
    {
        public int Lesson_Id { get; set; }
        public string Resource_Name { get; set; } = string.Empty;
        public string Resource_Description { get; set; } = string.Empty;
        public string YoutubeUrl { get; set; } = string.Empty;
    }
}
