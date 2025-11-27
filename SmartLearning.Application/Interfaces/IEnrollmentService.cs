

using SmartLearning.Application.DTOs.EnrollDto;

namespace SmartLearning.Application.Interfaces
{
    public interface IEnrollmentService
    {
        Task<EnrollmentResponseDto> EnrollAsync(EnrollmentRequestDto request);

        Task<bool> UnenrollAsync(int studentId, int courseId);

        Task<IEnumerable<CourseResponseDto>> GetStudentCoursesAsync(int studentId);
        Task<int> GetEnrollmentCountForCourseAsync(int courseId);
    }
}
