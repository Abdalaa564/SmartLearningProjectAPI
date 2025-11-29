

namespace SmartLearningProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController : ControllerBase
    {
       
        private readonly IEnrollmentService _enrollmentService;
        public EnrollmentController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }
        [HttpPost("enroll")]
        [ProducesResponseType(typeof(EnrollmentResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EnrollmentResponseDto>> EnrollStudent(
            [FromBody] EnrollmentRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _enrollmentService.EnrollStudentAsync(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("my-enrollments")]
        [ProducesResponseType(typeof(List<EnrollmentDetailsDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<EnrollmentDetailsDto>>> GetMyEnrollments()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var enrollments = await _enrollmentService.GetUserEnrollmentsAsync(userId);
          
            
          return Ok(enrollments);
        }

        [HttpGet("user/{userId}")]
        //[Authorize(Roles = "Admin,Instructor")]
        [ProducesResponseType(typeof(List<EnrollmentDetailsDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<EnrollmentDetailsDto>>> GetUserEnrollments(string userId)
        {
            var enrollments = await _enrollmentService.GetUserEnrollmentsAsync(userId);
            return Ok(enrollments);
        }

        [HttpGet("{enrollId}")]
        [ProducesResponseType(typeof(EnrollmentDetailsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EnrollmentDetailsDto>> GetEnrollment(int enrollId)
        {
            var enrollment = await _enrollmentService.GetEnrollmentByIdAsync(enrollId);

            if (enrollment == null)
                return NotFound(new { message = "Enrollment not found" });

            return Ok(enrollment);
        }

        [HttpGet("course/{courseId}")]
       // [Authorize(Roles = "Admin,Instructor")]
        [ProducesResponseType(typeof(List<EnrollmentDetailsDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<EnrollmentDetailsDto>>> GetCourseEnrollments(int courseId)
        {
            var enrollments = await _enrollmentService.GetCourseEnrollmentsAsync(courseId);
            return Ok(enrollments);
        }

        [HttpGet("check/{courseId}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> CheckEnrollment(int courseId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var isEnrolled = await _enrollmentService.IsStudentEnrolledAsync(userId, courseId);
            return Ok(isEnrolled);
        }
    }
}
