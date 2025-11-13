

namespace SmartLearning.Core.Model
{
    public class Lessons
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
