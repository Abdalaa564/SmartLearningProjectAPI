

namespace SmartLearningProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        // الطالب يعمل Check-In للدرس
        [HttpPost("checkin/{lessonId:int}")]
        // [Authorize(Roles = "Student")] // لو انت مفعّل roles
        [Authorize]
        public async Task<IActionResult> CheckIn(int lessonId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var success = await _attendanceService.CheckInAsync(lessonId, userId);
            if (!success)
                return BadRequest("Could not check in (maybe already checked in).");

            return Ok("Checked in successfully.");
        }

        // الطالب يعمل Check-Out (لو محتاج)
        [HttpPost("checkout/{lessonId:int}")]
        //  [Authorize(Roles = "Student")]
        [Authorize]
        public async Task<IActionResult> CheckOut(int lessonId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var success = await _attendanceService.CheckOutAsync(lessonId, userId);
            if (!success)
                return BadRequest("Could not check out.");

            return Ok("Checked out successfully.");
        }
        // GET: api/Attendance/lesson/5
        [HttpGet("lesson/{lessonId:int}")]
       // [Authorize(Roles = "Instructor")]
        [Authorize]
        public async Task<IActionResult> GetLessonAttendance(int lessonId)
        {
            var instructorUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(instructorUserId))
                return Unauthorized();

            var attendees = await _attendanceService.GetLessonAttendanceAsync(lessonId, instructorUserId);

            return Ok(attendees);
        }

    }
}