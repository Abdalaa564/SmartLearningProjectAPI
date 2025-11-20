

namespace SmartLearningProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorController : ControllerBase
    {
        private readonly IInstructorService _service;

        public InstructorController(IInstructorService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _service.GetAllInstructorAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _service.GetByIdAsync(id);
            return result is null ? NotFound() : Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateInstructorDto dto)
        {
            var instructor = await _service.AddInstructorAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = instructor.Id }, instructor);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, UpdateInstructorDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            return result ? Ok("Updated successfully") : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _service.DeleteAsync(id);
            return result ? Ok("Instructor deleted") : NotFound();
        }
    }
}