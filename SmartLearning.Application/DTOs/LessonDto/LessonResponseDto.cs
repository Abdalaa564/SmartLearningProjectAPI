
namespace SmartLearning.Application.DTOs.LessonDto
{
    public class LessonResponseDto
    {
        public int Lesson_Id { get; set; }
        public int Unit_Id { get; set; }
        public string Lesson_Name { get; set; } = string.Empty;
        public string LessonDescription { get; set; } = string.Empty;
        public List<ResourceResponseDto> Resources { get; set; } = new();
    }
}
