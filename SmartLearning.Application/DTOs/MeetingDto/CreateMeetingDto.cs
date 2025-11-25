
namespace SmartLearning.Application.DTOs.MeetingDto
{
    public class CreateMeetingDto
    {
        public DateTime StartsAt { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
