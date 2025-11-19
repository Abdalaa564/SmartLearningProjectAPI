
namespace SmartLearning.Application.Interfaces
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseResponseDTO>> GetAllCourseAsync();
    }
}
