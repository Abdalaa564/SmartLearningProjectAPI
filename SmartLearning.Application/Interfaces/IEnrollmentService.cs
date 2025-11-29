
using SmartLearning.Application.DTOs.EnrollmentDto;

namespace SmartLearning.Application.Interfaces
{
    public interface IEnrollmentService
    {
        Task<EnrollmentResponseDto> EnrollStudentAsync(EnrollmentRequestDto request);
        Task<List<EnrollmentDetailsDto>> GetUserEnrollmentsAsync(string userId);
        Task<EnrollmentDetailsDto?> GetEnrollmentByIdAsync(int enrollId);
        Task<List<EnrollmentDetailsDto>> GetCourseEnrollmentsAsync(int courseId);
        Task<bool> IsStudentEnrolledAsync(string userId, int courseId);
    }
}
