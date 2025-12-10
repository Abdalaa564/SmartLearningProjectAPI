
namespace SmartLearningProjectAPI.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IInstructorService _instructorService;
        private readonly IAdminService _adminService;

        public AdminController(IInstructorService instructorService, IAdminService adminService)
        {
            _instructorService = instructorService;
            _adminService = adminService;
        }

        // ========== Instructor Management ==========

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

        // ========== Admin Management ==========

        [HttpGet("admins")]
        public async Task<IActionResult> GetAllAdmins()
        {
            var admins = await _adminService.GetAllAsync();
            return Ok(admins);
        }

        [HttpGet("admins/{userId}")]
        public async Task<IActionResult> GetAdminById(string userId)
        {
            var admin = await _adminService.GetByIdAsync(userId);
            if (admin == null)
                return NotFound(new { message = "Admin not found" });
            return Ok(admin);
        }

        [HttpPost("admins")]
        public async Task<IActionResult> CreateAdmin([FromBody] CreateAdminDto dto)
        {
            try
            {
                var admin = await _adminService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetAdminById), new { userId = admin.Id }, admin);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("admins/{userId}")]
        public async Task<IActionResult> UpdateAdmin(string userId, [FromBody] UpdateAdminDto dto)
        {
            var success = await _adminService.UpdateAsync(userId, dto);
            if (!success)
                return NotFound(new { message = "Admin not found" });
            return Ok(new { message = "Admin updated successfully" });
        }

        [HttpDelete("admins/{userId}")]
        public async Task<IActionResult> DeleteAdmin(string userId)
        {
            try
            {
                var success = await _adminService.DeleteAsync(userId);
                if (!success)
                    return NotFound(new { message = "Admin not found" });
                return Ok(new { message = "Admin deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("admins/count")]
        public async Task<IActionResult> GetAdminsCount()
        {
            var count = await _adminService.GetAdminsCountAsync();
            return Ok(new { count });
        }
    }
}