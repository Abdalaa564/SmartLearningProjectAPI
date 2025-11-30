using SmartLearning.Application.DTOs.Attendance;

namespace SmartLearning.Application.Interfaces
{
    public interface IAttendanceService
    {
        Task<bool> CheckInAsync(int lessonId, string userId);
        Task<bool> CheckOutAsync(int lessonId, string userId);
        Task<IReadOnlyList<LessonAttendanceStudentDto>> GetLessonAttendanceAsync(int lessonId, string instructorUserId);
    }
}
