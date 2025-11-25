namespace SmartLearningProjectAPI.Controllers
{
    [ApiController]
    [Route("api/stream")]
    public class StreamController : ControllerBase
    {
        private readonly StreamTokenService _tokenService;

        public StreamController(StreamTokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpGet("token")]
        public IActionResult GetToken([FromQuery] string userId, [FromQuery] string callId)
        {
            if (string.IsNullOrEmpty(userId))
                return BadRequest("Missing userId");

            if (string.IsNullOrEmpty(callId))
                return BadRequest("Missing callId");

            var token = _tokenService.CreateUserToken(userId, "default", callId);
            return Ok(new { token });
        }
    }
}
