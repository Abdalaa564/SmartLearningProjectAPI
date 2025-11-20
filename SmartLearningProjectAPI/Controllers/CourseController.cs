



using SmartLearning.Application.DTOs.CourseDto;

namespace SmartLearningProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _courseService.GetAllCourseAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
            => Ok(await _courseService.GetByIdAsync(id));

        [HttpPost]
        public async Task<IActionResult> Create(AddCourseDto dto)
            => await _courseService.AddCourseAsync(dto)
                ? Ok("Created Successfully")
                : BadRequest("Failed");

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateCourseDto dto)
            => await _courseService.UpdateCourseAsync(id, dto)
                ? Ok("Updated Successfully")
                : NotFound("Not Found");

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
            => await _courseService.DeleteCourseAsync(id)
                ? Ok("Deleted Successfully")
                : NotFound("Not Found");
    }
}
