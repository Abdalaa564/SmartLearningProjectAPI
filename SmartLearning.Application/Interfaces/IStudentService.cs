

namespace SmartLearning.Application.Interfaces
{
    public interface IStudentService
    {
        Task<AuthResponseDto> RegisterStudentAsync(RegisterStudentDto registerDto);
        Task<StudentProfileDto> GetStudentProfileAsync(string userId);

        Task<StudentProfileDto> UpdateStudentAsync(string userId, StudentUpdateDto updateDto);

    }
}
