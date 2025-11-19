
namespace SmartLearning.Application.DTOs
{
    public class CourseResponseDTO
    {
        public int Crs_Id { get; set; }
        public string Crs_Name { get; set; } = string.Empty;
        public string Crs_Description { get; set; } = string.Empty;
        public decimal Price { get; set; }

        public string User_Id { get; set; } = string.Empty;
        public string InstructorName { get; set; } = string.Empty;
    }
}
