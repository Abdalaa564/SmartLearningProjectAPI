

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
        public async Task<ActionResult<StudentProfileDto>> UpdateProfile( StudentUpdateDto updateDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "User not found" });

                var profile = await _studentService.UpdateStudentAsync(userId, updateDto);
                return Ok(new { success = true, message = "Profile updated successfully", data = profile });
           
        }
    }
}


