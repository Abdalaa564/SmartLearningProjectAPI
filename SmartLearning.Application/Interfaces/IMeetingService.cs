
namespace SmartLearning.Application.Interfaces
{
    public interface IMeetingService
    {
        Task<MeetingResponseDto> CreateMeetingAsync(string userId, CreateMeetingDto dto);
        Task<List<MeetingResponseDto>> GetAllMeetingsAsync(string? userId = null);
        Task<MeetingResponseDto?> GetMeetingByIdAsync(Guid id);
        Task<bool> DeleteMeetingAsync(Guid id, string userId);
    }
}
