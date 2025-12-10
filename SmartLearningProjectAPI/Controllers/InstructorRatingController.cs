using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartLearning.Application.DTOs.Rating;

namespace SmartLearningProjectAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class InstructorRatingController : ControllerBase
	{
		private readonly IInstructorRatingService _service;

		public InstructorRatingController(IInstructorRatingService service)
		{
			_service = service;
		}

		[HttpPost]
		public async Task<IActionResult> AddRating([FromBody] InstructorRatingDto dto)
		{
			// Get UserId from Claims
			dto.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			if (string.IsNullOrEmpty(dto.UserId))
				return Unauthorized("Invalid Token");

			await _service.AddInstructorRatingAsync(dto);
			return Ok(new { Message = "Rating added successfully" });
		}

		[HttpGet("average/{instructorId}")]
		public async Task<IActionResult> GetAverage(int instructorId)
		{
			var avg = await _service.GetInstructorAverageRating(instructorId);
			return Ok(new { InstructorId = instructorId, Average = avg });
		}

		[HttpGet("instructor/{instructorId}")]
		public async Task<IActionResult> GetInstructorRatings(int instructorId)
		{
			var ratings = await _service.GetRatingsForInstructor(instructorId);
			return Ok(ratings);
		}
		[HttpGet("hasRated/{instructorId}")]
		public async Task<IActionResult> HasUserRated(int instructorId)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (string.IsNullOrEmpty(userId))
				return Unauthorized("Invalid Token");

			var hasRated = await _service.HasUserRatedInstructor(instructorId, userId);
			return Ok(new { HasRated = hasRated });
		}
	}
}
