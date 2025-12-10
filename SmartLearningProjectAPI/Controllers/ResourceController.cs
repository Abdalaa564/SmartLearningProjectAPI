namespace SmartLearningProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResourceController : ControllerBase
    {

        private readonly IResourceService _resourceService;
        private readonly IWebHostEnvironment _env;

        public ResourceController(IResourceService resourceService, IWebHostEnvironment env)
        {
            _resourceService = resourceService;
            _env = env;
        }

        // =========================
        // 1) ADD RESOURCE (JSON عام)
        // POST: api/Resource
        // =========================
        [Authorize(Roles = "Instructor,Admin")]
        [HttpPost]
        public async Task<IActionResult> AddResource([FromBody] CreateResourceDto dto)
        {
            var resource = await _resourceService.AddResourceAsync(dto);
            return Ok(resource);
        }

        // =========================
        // 2) UPLOAD PDF من الجهاز
        // POST: api/Resource/upload-pdf
        // =========================
        [HttpPost("upload-pdf")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadPdf(
    [FromForm] int lessonId,
    [FromForm] string resourceName,
    [FromForm] string? resourceDescription,
    IFormFile file) // لاحظ: مفيش [FromForm] هنا
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is required.");

            if (!file.ContentType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase))
                return BadRequest("Only PDF files are allowed.");

            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "pdfs");
            Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var fileUrl = $"{baseUrl}/uploads/pdfs/{fileName}";

            var pdfIconUrl = $"{baseUrl}/images/pdf-icon.png"; // أي صورة pdf-icon.png في wwwroot/images

            var dto = new CreateResourceDto
            {
                Lesson_Id = lessonId,
                Resource_Name = resourceName,
                Resource_Description = resourceDescription ?? string.Empty,
                Resource_Url = fileUrl,
                Resource_Type = "pdf",
                ThumbnailUrl = pdfIconUrl
            };

            var resource = await _resourceService.AddResourceAsync(dto);
            return Ok(resource);
        }


        // =========================
        // 3) ADD YOUTUBE RESOURCE
        // POST: api/Resource/youtube
        // =========================
        [HttpPost("youtube")]
        public async Task<IActionResult> AddYoutubeResource([FromBody] CreateYoutubeResourceDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.YoutubeUrl))
                return BadRequest("YoutubeUrl is required.");

            var videoId = ExtractYoutubeVideoId(dto.YoutubeUrl);
            if (string.IsNullOrEmpty(videoId))
                return BadRequest("Invalid YouTube URL.");

            // Thumbnail الرسمي من YouTube
            var thumbnailUrl = $"https://img.youtube.com/vi/{videoId}/hqdefault.jpg";

            var createResourceDto = new CreateResourceDto
            {
                Lesson_Id = dto.Lesson_Id,
                Resource_Name = dto.Resource_Name,
                Resource_Description = dto.Resource_Description,
                Resource_Url = dto.YoutubeUrl,   // اللينك اللي هيتفتح
                Resource_Type = "video",
                ThumbnailUrl = thumbnailUrl      // الصورة اللي هتظهر في الفرونت
            };

            var resource = await _resourceService.AddResourceAsync(createResourceDto);
            return Ok(resource);
        }

        // =========================
        // 4) GET BY ID
        // GET: api/Resource/{id}
        // =========================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetResource(int id)
        {
            var resource = await _resourceService.GetByIdAsync(id);
            if (resource == null) return NotFound();
            return Ok(resource);
        }

        // =========================
        // 5) GET BY LESSON
        // GET: api/Resource/lesson/{lessonId}
        // =========================
        [HttpGet("lesson/{lessonId}")]
        public async Task<IActionResult> GetResourcesByLesson(int lessonId)
        {
            var resources = await _resourceService.GetByLessonIdAsync(lessonId);
            return Ok(resources);
        }

        // =========================
        // 6) DELETE
        // DELETE: api/Resource/{id}
        // =========================
        [Authorize(Roles = "Instructor,Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResource(int id)
        {
            await _resourceService.DeleteAsync(id);
            return NoContent();
        }

        // =========================
        // HELPER: استخراج VideoId من YouTube
        // =========================
        private string? ExtractYoutubeVideoId(string url)
        {
            if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
                return null;

            // https://youtu.be/{id}
            if (uri.Host.Contains("youtu.be"))
            {
                return uri.AbsolutePath.Trim('/');
            }

            // https://www.youtube.com/watch?v={id}
            var query = QueryHelpers.ParseQuery(uri.Query);
            if (query.TryGetValue("v", out var v))
            {
                return v.ToString();
            }

            return null;
        }
    }
}