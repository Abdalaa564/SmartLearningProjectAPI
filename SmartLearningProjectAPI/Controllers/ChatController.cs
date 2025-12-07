
namespace SmartLearningProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatGPTService _chatService;
        private readonly IPdfChatService _pdfChatService;

        public ChatController(IChatGPTService chatService, IPdfChatService pdfChatService)
        {
            _chatService = chatService;
            _pdfChatService = pdfChatService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Ask([FromBody] ChatRequest request)
        {
            var response = await _chatService.AskChatGPTAsync(request.Messages);
            return Ok(response);
        }

        [Authorize]
        [HttpPost("upload-pdf")]
        public async Task<IActionResult> UploadPdf(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            string text = await _pdfChatService.ExtractTextAsync(file);

            if (string.IsNullOrWhiteSpace(text))
                return BadRequest("Cannot read PDF");

            var messages = new List<ChatGPTMessage>
            {
                new ChatGPTMessage { role = "user", content = $"Summarize this PDF:\n{text}" }
            };

            var response = await _chatService.AskChatGPTAsync(messages);
            return Ok(response);
        }
    }
}
