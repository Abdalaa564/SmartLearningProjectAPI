


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
        [HttpPost("enroll")]
        [ProducesResponseType(typeof(EnrollmentInitiationResponseDto), StatusCodes.Status200OK)]

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EnrollmentInitiationResponseDto>> EnrollStudent(
             [FromBody] EnrollmentRequestDto request)
        {
            //if (!ModelState.IsValid)
            //    return BadRequest(ModelState);

            var result = await _enrollmentService.EnrollStudentAsync(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
        [HttpGet("paymob-callback")]

        [HttpPost("paymob-callback")]
        public async Task<IActionResult> PaymobCallback()
        {
            var callbackData = new Dictionary<string, string>();
            // Try query parameters first (GET request)
            if (Request.Query.Any())
            {
                foreach (var kvp in Request.Query)
                {
                    callbackData[kvp.Key] = kvp.Value.ToString();
                }
            }
            // Try form data (POST request)
            else if (Request.HasFormContentType && Request.Form.Any())
            {
                foreach (var kvp in Request.Form)
                {
                    callbackData[kvp.Key] = kvp.Value.ToString();
                }
            }

            // Process the payment callback
            var result = await _enrollmentService.CompleteEnrollmentFromCallbackAsync(callbackData);

            // Redirect to Angular frontend with result
            var frontendUrl = "http://localhost:4200/Courses"; // Update with your Angular URL

            if (!result.Success)
            {
                return Redirect($"{frontendUrl}?success=false&message={Uri.EscapeDataString(result.Message)}");
            }

            return Redirect($"{frontendUrl}?success=true&transactionId={result.TransactionId}&enrollmentId={result.EnrollmentId}");
        }
        [HttpPost("paymob-webhook")]
        public async Task<IActionResult> PaymobWebhook([FromForm] Dictionary<string, string> webhookData)
        { 
            
                var result = await _enrollmentService.CompleteEnrollmentFromCallbackAsync(webhookData);

                // Paymob expects 200 OK response
                return Ok(new { received = true });
           
        }


        [HttpGet("paymob-status/{transactionId}")]
        public async Task<IActionResult> GetEnrollmentStatus(string transactionId)
        {
            var status = await _enrollmentService.GetEnrollmentStatusAsync(transactionId);
            return Ok(status);
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




}
