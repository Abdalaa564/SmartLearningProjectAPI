
namespace SmartLearningProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetingsController : ControllerBase
    {
        private readonly IMeetingService _meetingService;

        public MeetingsController(IMeetingService meetingService)
        {
            _meetingService = meetingService;
        }

        private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateMeeting(CreateMeetingDto dto)
        {
            var meeting = await _meetingService.CreateMeetingAsync(UserId, dto);
            return Ok(meeting);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var meetings = await _meetingService.GetAllMeetingsAsync(UserId);
            return Ok(meetings);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var meeting = await _meetingService.GetMeetingByIdAsync(id);
            if (meeting == null) return NotFound();

            if (meeting.CreatedBy != UserId)
                return Forbid();

            return Ok(meeting);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _meetingService.DeleteMeetingAsync(id, UserId);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}
