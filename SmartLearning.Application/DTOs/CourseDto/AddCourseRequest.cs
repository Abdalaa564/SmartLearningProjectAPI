



namespace SmartLearning.Application.DTOs.CourseDto
{
    public class AddCourseRequest
    {
        [Required, MaxLength(100)]
        public string Crs_Name { get; set; }

        [MaxLength(500)]
        public string Crs_Description { get; set; }

        [Range(0, 99999)]
        public decimal Price { get; set; }

        [Required]
        public int InstructorId { get; set; }

        public IFormFile? ImageFile { get; set; }

        [MaxLength(300)]
        public string? ImageUrl { get; set; }
    }
}
