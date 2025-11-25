
namespace SmartLearning.Application.DTOs.MeetingDto
{
    public class MeetingResponseDto
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime StartsAt { get; set; }
        public string JoinLink { get; set; } = string.Empty;

        public string CreatedBy { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
    }
}
