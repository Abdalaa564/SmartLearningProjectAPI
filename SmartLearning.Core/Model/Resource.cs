
namespace SmartLearning.Core.Model
{
    public class Resource
    {
        [Key]
        public int Resource_Id { get; set; }

        [Required]
        public int Lesson_Id { get; set; }

        [Required, MaxLength(100)]
        public string Resource_Name { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string Resource_Description { get; set; } = string.Empty;

        [Required, MaxLength(500)]
        public string Resource_Url { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Resource_Type { get; set; } = string.Empty;
        [MaxLength(500)]
        public string? ThumbnailUrl { get; set; }

        public Lessons Lesson { get; set; } = null!;
    }
}
