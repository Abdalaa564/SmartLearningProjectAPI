


using SmartLearning.Application.DTOs.EnrollmentDto;

namespace SmartLearning.Application.Interfaces
{
    public interface IEnrollmentService
    {
      //  Task<EnrollmentResponseDto> EnrollAsync(EnrollmentRequestDto request);

        Task<bool> UnenrollAsync(int studentId, int courseId);
        Task<EnrollmentInitiationResponseDto> EnrollStudentAsync(EnrollmentRequestDto request);
        Task<EnrollmentDetailsDto?> GetEnrollmentByIdAsync(int enrollId);

        Task<IEnumerable<CourseResponseDto>> GetStudentCoursesAsync(int studentId);
        Task<int> GetEnrollmentCountForCourseAsync(int courseId);

        Task<int> GetNotEnrolledCountForCourseAsync(int courseId);


       // Task<EnrollmentResponseDto> InitiateEnrollmentAsync(EnrollmentRequestDto request);
        Task<EnrollmentResponseDto> CompleteEnrollmentFromCallbackAsync(Dictionary<string, string> callbackData);
        Task<EnrollmentStatusDto> GetEnrollmentStatusAsync(string transactionId);
        Task<EnrollmentDetailsDto> GetEnrollmentDetailsAsync(int enrollmentId);

    }
}
