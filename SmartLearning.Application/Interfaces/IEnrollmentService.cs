

using SmartLearning.Application.DTOs.EnrollmentDto;


namespace SmartLearning.Application.Interfaces
{
    public interface IEnrollmentService
    {

      //  Task<EnrollmentResponseDto> EnrollAsync(EnrollmentRequestDto request);

        Task<bool> UnenrollAsync(int studentId, int courseId);

        Task<IEnumerable<CourseResponseDto>> GetStudentCoursesAsync(int studentId);
        Task<int> GetEnrollmentCountForCourseAsync(int courseId);



        Task<EnrollmentResponseDto> EnrollStudentAsync(EnrollmentRequestDto request);
      //  Task<List<EnrollmentDetailsDto>> GetUserEnrollmentsAsync(string userId);
        Task<EnrollmentDetailsDto?> GetEnrollmentByIdAsync(int enrollId);
        //Task<List<EnrollmentDetailsDto>> GetCourseEnrollmentsAsync(int courseId);
        Task<bool> IsStudentEnrolledAsync(int userId, int courseId);

    }
}
