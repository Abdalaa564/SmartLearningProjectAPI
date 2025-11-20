
namespace SmartLearning.Application.DTOs.LessonDto
{
    public class CreateLessonDto
    {
        public int Unit_Id { get; set; }
        public string Lesson_Name { get; set; } = string.Empty;
        public string LessonDescription { get; set; } = string.Empty;
    }
}
