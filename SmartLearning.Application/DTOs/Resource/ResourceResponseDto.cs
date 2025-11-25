

namespace SmartLearning.Application.DTOs.Resource
{
    public class ResourceResponseDto
    {
        public int Resource_Id { get; set; }
        public int Lesson_Id { get; set; }
        public string Resource_Name { get; set; } = string.Empty;
        public string Resource_Description { get; set; } = string.Empty;
        public string Resource_Url { get; set; } = string.Empty;
        public string Resource_Type { get; set; } = string.Empty;
        public string? ThumbnailUrl { get; set; }
    }
}
