
namespace SmartLearningProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly IWebHostEnvironment _env;


        public CourseController(ICourseService courseService, IWebHostEnvironment env)
        {
            _courseService = courseService;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _courseService.GetAllCourseAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
            => Ok(await _courseService.GetByIdAsync(id));

        [HttpPost]
        [Consumes("multipart/form-data")] 
        public async Task<IActionResult> Create([FromForm] AddCourseRequest request)
        {
            string? finalImageUrl = null;

            if (request.ImageFile is not null && request.ImageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "courses");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(request.ImageFile.FileName)}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.ImageFile.CopyToAsync(stream);
                }

                finalImageUrl = $"/images/courses/{fileName}";
            }
            else if (!string.IsNullOrWhiteSpace(request.ImageUrl))
            {
                finalImageUrl = request.ImageUrl;
            }

            var dto = new AddCourseDto
            {
                Crs_Name = request.Crs_Name,
                Crs_Description = request.Crs_Description,
                Price = request.Price,
                InstructorId = request.InstructorId,
                ImageUrl = finalImageUrl
            };

            var isCreated = await _courseService.AddCourseAsync(dto);

            if (!isCreated)
                return BadRequest("Failed");

            return Ok("Created Successfully");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateCourseDto dto)
        {
            string? uploadedImagePath = null;

            if (dto.ImageFile != null && dto.ImageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "images", "courses");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.ImageFile.FileName)}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.ImageFile.CopyToAsync(stream);
                }

                uploadedImagePath = $"/images/courses/{fileName}";
            }

            var result = await _courseService.UpdateCourseAsync(id, dto, uploadedImagePath);

            return result
                ? Ok("Updated Successfully")
                : NotFound("Not Found");
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
            => await _courseService.DeleteCourseAsync(id)
                ? Ok("Deleted Successfully")
                : NotFound("Not Found");
    }
}
