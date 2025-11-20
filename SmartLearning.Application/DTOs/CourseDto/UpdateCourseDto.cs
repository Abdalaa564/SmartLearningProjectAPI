

namespace SmartLearning.Application.DTOs.CourseDto
{
    public class UpdateCourseDto
    {
        [Required, MaxLength(100)]
        public string Crs_Name { get; set; }

        [MaxLength(500)]
        public string Crs_Description { get; set; }

        [Range(0, 99999)]
        public decimal Price { get; set; }
    }
}
