
namespace SmartLearningProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitController : ControllerBase
    {
        private readonly IUnitService _unitService;

        public UnitController(IUnitService unitService)
        {
            _unitService = unitService;
        }

        // POST: api/Unit
        [Authorize(Roles = "Instructor")]
        [HttpPost]
        public async Task<IActionResult> AddUnit([FromBody] CreateUnitDto dto)
        {
            var unit = await _unitService.AddUnitAsync(dto);
            return Ok(unit);
        }

        // GET: api/Unit/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUnit(int id)
        {
            var unit = await _unitService.GetUnitByIdAsync(id);
            if (unit == null) return NotFound();
            return Ok(unit);
        }

        // GET: api/Unit/course/{courseId}
        [HttpGet("course/{courseId}")]
        public async Task<IActionResult> GetUnitsByCourse(int courseId)
        {
            var units = await _unitService.GetUnitsByCourseIdAsync(courseId);
            return Ok(units);
        }

        // PUT: api/Unit/{id}
        [Authorize(Roles = "Instructor")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUnit(int id, [FromBody] UpdateUnitDto dto)
        {
            try
            {
                await _unitService.UpdateUnitAsync(id, dto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // DELETE: api/Unit/{id}
        [Authorize(Roles = "Instructor")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUnit(int id)
        {
            await _unitService.DeleteUnitAsync(id);
            return NoContent();
        }
    }
}
