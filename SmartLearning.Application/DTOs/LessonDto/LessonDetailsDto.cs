
using SmartLearning.Application.DTOs.Resource;
namespace SmartLearning.Application.DTOs.LessonDto
{
    public class LessonDetailsDto
    {
        public int Lesson_Id { get; set; }
        public int Unit_Id { get; set; }
        public string Lesson_Name { get; set; } = string.Empty;
        public string LessonDescription { get; set; } = string.Empty;

        // هنا هتيجي كل الـ Resources المرتبطة بالدرس (PDF + YouTube)
        public List<ResourceResponseDto> Resources { get; set; } = new();
    }
}
