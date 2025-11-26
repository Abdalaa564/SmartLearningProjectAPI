
using SmartLearning.Application.DTOs.ChatGPT;

namespace SmartLearningProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatGPTService _chatService;

        public ChatController(IChatGPTService chatService)
        {
            _chatService = chatService;
        }

        //[HttpPost]
        //public async Task<IActionResult> Ask([FromBody] string prompt)
        //{
        //    var response = await _chatService.AskChatGPTAsync(prompt);
        //    return Ok(response);
        //}

        [HttpPost]
        public async Task<IActionResult> Ask([FromBody] ChatRequest request)
        {
            var response = await _chatService.AskChatGPTAsync(request.Messages);
            return Ok(response);
        }
    }
}
