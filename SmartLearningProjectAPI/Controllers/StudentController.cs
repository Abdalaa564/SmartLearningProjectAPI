
namespace SmartLearningProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }



        [HttpGet("profile/{userId}")]

        public async Task<ActionResult<StudentProfileDto>> GetStudentProfileById(string userId)
        {

            var profile = await _studentService.GetStudentProfileAsync(userId);
            return Ok(profile);

        }

        [HttpPut("profile")]
        public async Task<ActionResult<StudentProfileDto>> UpdateProfile(StudentUpdateDto updateDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User not found" });

            var profile = await _studentService.UpdateStudentAsync(userId, updateDto);
            return Ok(new { success = true, message = "Profile updated successfully", data = profile });

        }

        [HttpDelete("{userId}")]
       // [Authorize(Roles = "Admin")]

        public async Task<ActionResult> DeleteStudent(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return BadRequest(new { message = "Student ID is required" });

            var result = await _studentService.DeleteStudentAsync(userId);

            return Ok(new
            {
                success = true,
                message = "Student deleted successfully"
            });
        }
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<StudentProfileDto>>> GetAllStudents()
        {
            var students = await _studentService.GetAllStudentsAsync();

            return Ok(new
            {
                success = true,
                total = students.Count(),
                data = students
            });
        }

        // number of all student 
        [HttpGet("count")]
        public async Task<IActionResult> GetStudentsCount()
        {
            var count = await _studentService.GetStudentsCountAsync();
            return Ok(new { totalStudents = count });
        }
    }
}


