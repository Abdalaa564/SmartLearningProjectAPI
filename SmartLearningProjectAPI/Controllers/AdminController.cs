
namespace SmartLearningProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IInstructorService _instructorService;

        public AdminController(IInstructorService instructorService)
        {
            _instructorService = instructorService;
        }

        [HttpGet("pending-instructors")]
        public async Task<IActionResult> GetPendingInstructors()
        {
            var pending = await _instructorService.GetPendingInstructorsAsync();
            return Ok(pending);
        }

        [HttpPost("instructors/{id:int}/approve")]
        public async Task<IActionResult> ApproveInstructor(int id)
        {
            var success = await _instructorService.ApproveInstructorAsync(id);
            if (!success)
                return NotFound(new { message = "Instructor not found" });

            return Ok(new { message = "Instructor approved successfully" });
        }

        [HttpPost("instructors/{id:int}/reject")]
        public async Task<IActionResult> RejectInstructor(int id)
        {
            var success = await _instructorService.RejectInstructorAsync(id);
            if (!success)
                return NotFound(new { message = "Instructor not found" });

            return Ok(new { message = "Instructor rejected successfully" });
        }
    }
}