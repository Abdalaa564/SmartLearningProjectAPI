
namespace SmartLearning.Application.DTOs.CourseDto
{
    public class CourseResponseDto
    {
        public int Crs_Id { get; set; }
        public string Crs_Name { get; set; } = string.Empty;
        public string Crs_Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int InstructorId { get; set; } 
        public string InstructorName { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public string? InstructorPhoto { get; set; }

        public List<CourseStudentDto> EnrolledStudents { get; set; } = new();

    }
}
