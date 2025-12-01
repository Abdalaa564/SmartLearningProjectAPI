


using SmartLearning.Application.DTOs.EnrollmentDto;

namespace SmartLearning.Application.Interfaces
{
    public interface IEnrollmentService
    {
        Task<EnrollmentResponseDto> EnrollAsync(EnrollmentRequestDto request);

        Task<bool> UnenrollAsync(int studentId, int courseId);

        Task<IEnumerable<CourseResponseDto>> GetStudentCoursesAsync(int studentId);
        Task<int> GetEnrollmentCountForCourseAsync(int courseId);

        Task<int> GetNotEnrolledCountForCourseAsync(int courseId);
    }
}
