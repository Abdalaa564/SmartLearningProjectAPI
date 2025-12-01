


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

        // POST: api/Enrollment
        // Body:
        // {
        //   "studentId": 1,
        //   "courseId": 5,
        //   "transactionId": "string"
        // }
        [HttpPost]
        public async Task<IActionResult> Enroll([FromBody] EnrollmentRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _enrollmentService.EnrollAsync(request);
            return Ok(result);
        }

        // DELETE: api/Enrollment/{studentId}/{courseId}
        [HttpDelete("{studentId:int}/{courseId:int}")]
        public async Task<IActionResult> UnEnroll(int studentId, int courseId)
        {
            var success = await _enrollmentService.UnenrollAsync(studentId, courseId);

            if (!success)
                return NotFound(new { success = false, message = "Enrollment not found for this student/course" });

            return Ok(new { success = true, message = "Unenrolled successfully" });
        }

        // GET: api/Enrollment/student/{studentId}
        [HttpGet("student/{studentId:int}")]
        public async Task<IActionResult> GetStudentCourses(int studentId)
        {
            var courses = await _enrollmentService.GetStudentCoursesAsync(studentId);
            return Ok(courses);
        }



        [HttpGet("course/{courseId}/count")]
        public async Task<IActionResult> GetEnrollmentCount(int courseId)
        {
            var count = await _enrollmentService.GetEnrollmentCountForCourseAsync(courseId);

            return Ok(new
            {
                CourseId = courseId,
                EnrollmentCount = count
            });
        }
        // ✅ All students not enrolled in a specific course
        [HttpGet("course/{courseId:int}/not-enrolled-count")]
        public async Task<IActionResult> GetNotEnrolledCount(int courseId)
        {
            var count = await _enrollmentService.GetNotEnrolledCountForCourseAsync(courseId);

            return Ok(new
            {
                CourseId = courseId,
                NotEnrolledCount = count
            });
        }
    }

    //[HttpDelete("{studentId:int}/{courseId:int}")]
    ////   [Authorize]
    //public async Task<IActionResult> UnEnroll(int studentId, int courseId)
    //{
    //    var success = await _enrollmentService.UnenrollAsync(studentId, courseId);

    //    if (!success)
    //        return NotFound(new { success = false, message = "Enrollment not found for this student/course" });

    //    return Ok(new { success = true, message = "Unenrolled successfully" });
    //}


    ////Check if current student is enrolled in a course
    //// GET: api/Enrollment/check/{courseId}
    //[HttpGet("check/{courseId}")]
    //[Authorize]
    //[ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    //public async Task<ActionResult<bool>> CheckEnrollment(int courseId)
    //{
    //    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    //    if (string.IsNullOrEmpty(userId))
    //        return Unauthorized();

    //    if (!int.TryParse(userId, out int studentId))
    //        return BadRequest("Invalid user ID format.");

    //    var isEnrolled = await _enrollmentService.IsStudentEnrolledAsync(studentId, courseId);
    //    return Ok(isEnrolled);
    //}


    // Get all courses for current student
    // GET: api/Enrollment/student/my-courses
    // [HttpGet("student/my-courses")]
    //// [Authorize]
    // [ProducesResponseType(typeof(IEnumerable<CourseResponseDto>), StatusCodes.Status200OK)]
    // public async Task<IActionResult> GetMyCourses()
    // {
    //     var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    //     if (string.IsNullOrEmpty(userId))
    //         return Unauthorized();

    //     var studentId = userId; // Assuming studentId == userId
    //     var courses = await _enrollmentService.GetStudentCoursesAsync(studentId);
    //     return Ok(courses);
    // }



    // Get courses for specific student (Admin/Instructor)
    // GET: api/Enrollment/student/{studentId}/courses
    //   [HttpGet("student/{studentId}/courses")]
    ////   [Authorize(Roles = "Admin,Instructor")]
    //   [ProducesResponseType(typeof(IEnumerable<CourseResponseDto>), StatusCodes.Status200OK)]
    //   public async Task<IActionResult> GetStudentCourses(int studentId)
    //   {
    //       var courses = await _enrollmentService.GetStudentCoursesAsync(studentId);
    //       return Ok(courses);
    //   }




    // Unenroll from a course    
    // DELETE: api/Enrollment/{studentId}/{courseId}




}
