
namespace SmartLearningProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonController : ControllerBase
    {
        private readonly ILessonService _lessonService;

        public LessonController(ILessonService lessonService)
        {
            _lessonService = lessonService;
        }

        // POST: api/Lesson
        [HttpPost]
        public async Task<IActionResult> AddLesson([FromBody] CreateLessonDto dto)
        {
            var lesson = await _lessonService.AddLessonAsync(dto);
            return Ok(lesson);
        }

        // GET: api/Lesson/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLesson(int id)
        {
            var lesson = await _lessonService.GetLessonByIdAsync(id);
            if (lesson == null) return NotFound();
            return Ok(lesson);
        }

        // GET: api/Lesson/unit/{unitId}
        [HttpGet("unit/{unitId}")]
        public async Task<IActionResult> GetLessonsByUnit(int unitId)
        {
            var lessons = await _lessonService.GetLessonsByUnitIdAsync(unitId);
            return Ok(lessons);
        }

        // PUT: api/Lesson/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLesson(int id, [FromBody] UpdateLessonDto dto)
        {
            try
            {
                await _lessonService.UpdateLessonAsync(id, dto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // DELETE: api/Lesson/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLesson(int id)
        {
            await _lessonService.DeleteLessonAsync(id);
            return NoContent();
        }
    }
}