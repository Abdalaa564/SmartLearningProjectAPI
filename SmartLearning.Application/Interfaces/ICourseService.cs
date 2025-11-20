

namespace SmartLearning.Application.Interfaces
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseResponseDto>> GetAllCourseAsync();
        Task<CourseResponseDto?> GetByIdAsync(int id);
        Task<bool> AddCourseAsync(AddCourseDto dto);
        Task<bool> UpdateCourseAsync(int id, UpdateCourseDto dto);
        Task<bool> DeleteCourseAsync(int id);
    }
}
