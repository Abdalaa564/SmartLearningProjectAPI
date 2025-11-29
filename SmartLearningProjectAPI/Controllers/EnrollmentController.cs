


using SmartLearning.Application.DTOs.EnrollmentDto;

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

       
        // Enroll a student in a course with payment
        // POST: api/Enrollment/enroll
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




        // Get current student's enrollments
        // GET: api/Enrollment/my-enrollments
        [HttpGet("my-enrollments")]
       // [Authorize]
        [ProducesResponseType(typeof(List<EnrollmentDetailsDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<EnrollmentDetailsDto>>> GetMyEnrollments()
        {
            // Get the logged-in user's ID
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            // Find the student record for this user
            // You'll need to implement GetStudentIdByUserIdAsync in your service
            var studentId = userId; // Assuming studentId == userId for simplicity

            var enrollments = await _enrollmentService.GetStudentCoursesAsync(studentId);
            return Ok(enrollments);
        }

    
        // Get student enrollments by studentId (Admin/Instructor)
        // GET: api/Enrollment/student/{studentId}
        [HttpGet("student/{studentId}")]
      //  [Authorize(Roles = "Admin,Instructor")]
        [ProducesResponseType(typeof(List<EnrollmentDetailsDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<EnrollmentDetailsDto>>> GetStudentEnrollments(string studentId)
        {
            var enrollments = await _enrollmentService.GetStudentCoursesAsync(studentId);
            return Ok(enrollments);
        }





        


        

        // Get all enrollments for a course (Admin/Instructor)
        // GET: api/Enrollment/course/{courseId}
        [HttpGet("course/{courseId}")]
      //  [Authorize(Roles = "Admin,Instructor")]
        [ProducesResponseType(typeof(List<EnrollmentDetailsDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<EnrollmentDetailsDto>>> GetCourseEnrollments(int courseId)
        {
            var enrollments = await _enrollmentService.GetEnrollmentCountForCourseAsync(courseId);
            return Ok(enrollments);
        }

      
        // Get enrollment count for a course
        // GET: api/Enrollment/course/{courseId}/count
        [HttpGet("course/{courseId}/count")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEnrollmentCount(int courseId)
        {
            var count = await _enrollmentService.GetEnrollmentCountForCourseAsync(courseId);

            return Ok(new
            {
                CourseId = courseId,
                EnrollmentCount = count
            });
        }



        
        // Check if current student is enrolled in a course
        // GET: api/Enrollment/check/{courseId}
        [HttpGet("check/{courseId}")]
        [Authorize]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> CheckEnrollment(int courseId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var studentId = userId; // Assuming studentId == userId
            var isEnrolled = await _enrollmentService.IsStudentEnrolledAsync(studentId, courseId);
            return Ok(isEnrolled);
        }

     
        // Get all courses for current student
        // GET: api/Enrollment/student/my-courses
        [HttpGet("student/my-courses")]
       // [Authorize]
        [ProducesResponseType(typeof(IEnumerable<CourseResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMyCourses()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var studentId = userId; // Assuming studentId == userId
            var courses = await _enrollmentService.GetStudentCoursesAsync(studentId);
            return Ok(courses);
        }

       

        // Get courses for specific student (Admin/Instructor)
        // GET: api/Enrollment/student/{studentId}/courses
        [HttpGet("student/{studentId}/courses")]
     //   [Authorize(Roles = "Admin,Instructor")]
        [ProducesResponseType(typeof(IEnumerable<CourseResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStudentCourses(string studentId)
        {
            var courses = await _enrollmentService.GetStudentCoursesAsync(studentId);
            return Ok(courses);
        }

       


        // Unenroll from a course    
        // DELETE: api/Enrollment/{studentId}/{courseId}
        [HttpDelete("{studentId:int}/{courseId:int}")]
        //   [Authorize]
        public async Task<IActionResult> UnEnroll(int studentId, int courseId)
        {
            var success = await _enrollmentService.UnenrollAsync(studentId, courseId);

            if (!success)
                return NotFound(new { success = false, message = "Enrollment not found for this student/course" });

            return Ok(new { success = true, message = "Unenrolled successfully" });
        }
    }
}
