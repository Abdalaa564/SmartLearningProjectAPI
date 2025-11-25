
namespace SmartLearningProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
 
        public class LessonController : ControllerBase
        {
            private readonly ILessonService _lessonService;
            private readonly IResourceService _resourceService;
            private readonly IWebHostEnvironment _env;

            public LessonController(
                ILessonService lessonService,
                IResourceService resourceService,
                IWebHostEnvironment env)
            {
                _lessonService = lessonService;
                _resourceService = resourceService;
                _env = env;
            }

            // ============================================================
            // 1) إنشاء درس + PDF (إجباري) + YouTube (اختياري)
            // POST: api/Lesson/create
            // ============================================================
            [HttpPost("create")]
            [Consumes("multipart/form-data")]
            public async Task<IActionResult> CreateLessonWithResources(
                [FromForm] int unitId,
                [FromForm] string lessonName,
                [FromForm] string? lessonDescription,
                IFormFile file,                         // PDF (مطلوب)
                [FromForm] string? youtubeUrl           // YouTube (اختياري)
            )
            {
                // 1) تحقق من وجود الـ PDF
                if (file == null || file.Length == 0)
                    return BadRequest("PDF file is required.");

                if (!file.ContentType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase))
                    return BadRequest("Only PDF files are allowed.");

                // 2) نعمل الدرس الأول
                var lessonDto = new CreateLessonDto
                {
                    Unit_Id = unitId,
                    Lesson_Name = lessonName,
                    LessonDescription = lessonDescription ?? string.Empty
                };

                var lesson = await _lessonService.AddLessonAsync(lessonDto);

                var createdResources = new List<ResourceResponseDto>();

                // 3) حفظ الـ PDF وإنشاء Resource له
                var uploadsFolder = Path.Combine(
                    _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"),
                    "uploads", "pdfs");

                Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                var fileUrl = $"{baseUrl}/uploads/pdfs/{fileName}";
                var pdfIconUrl = $"{baseUrl}/images/pdf-icon.png"; // أيقونة ثابتة للـ PDF

                var pdfResourceDto = new CreateResourceDto
                {
                    Lesson_Id = lesson.Lesson_Id,
                    Resource_Name = $"{lessonName} - PDF",
                    Resource_Description = "Lesson PDF file",
                    Resource_Url = fileUrl,
                    Resource_Type = "pdf",
                    ThumbnailUrl = pdfIconUrl
                };

                var pdfResource = await _resourceService.AddResourceAsync(pdfResourceDto);
                createdResources.Add(pdfResource);

                // 4) لو YouTube مبعوت → نضيفه كـ Resource تاني (اختياري)
                if (!string.IsNullOrWhiteSpace(youtubeUrl))
                {
                    var videoId = ExtractYoutubeVideoId(youtubeUrl);
                    if (string.IsNullOrEmpty(videoId))
                        return BadRequest("Invalid YouTube URL.");

                    var thumbnailUrl = $"https://img.youtube.com/vi/{videoId}/hqdefault.jpg";

                    var youtubeResourceDto = new CreateResourceDto
                    {
                        Lesson_Id = lesson.Lesson_Id,
                        Resource_Name = $"{lessonName} - Video",
                        Resource_Description = "Lesson YouTube video",
                        Resource_Url = youtubeUrl,
                        Resource_Type = "video",
                        ThumbnailUrl = thumbnailUrl
                    };

                    var youtubeResource = await _resourceService.AddResourceAsync(youtubeResourceDto);
                    createdResources.Add(youtubeResource);
                }

                // 5) رجّع الدرس + كل الـ Resources المرتبطة بيه
                return Ok(new
                {
                    Lesson = lesson,
                    Resources = createdResources
                });
            }

        // GET: api/Lesson/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLesson(int id)
        {
            var lesson = await _lessonService.GetLessonWithResourcesByIdAsync(id);
            if (lesson == null) return NotFound();
            return Ok(lesson);
        }
        [HttpGet("unit/{unitId}")]
            public async Task<IActionResult> GetLessonsByUnit(int unitId)
            {
                var lessons = await _lessonService.GetLessonsByUnitIdAsync(unitId);
                return Ok(lessons);
            }

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

            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteLesson(int id)
            {
                await _lessonService.DeleteLessonAsync(id);
                return NoContent();
            }
            private string? ExtractYoutubeVideoId(string url)
            {
                if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
                    return null;

                if (uri.Host.Contains("youtu.be"))
                {
                    return uri.AbsolutePath.Trim('/');
                }

                var query = QueryHelpers.ParseQuery(uri.Query);
                if (query.TryGetValue("v", out var v))
                {
                    return v.ToString();
                }

                return null;
            }
        }
    }