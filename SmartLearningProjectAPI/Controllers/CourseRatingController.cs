using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartLearning.Application.DTOs.Rating;

namespace SmartLearningProjectAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class CourseRatingController : ControllerBase
	{
		private readonly ICourseRatingService _courseRatingService;

		public CourseRatingController(ICourseRatingService courseRatingService)
		{
			_courseRatingService = courseRatingService;
		}

		[HttpPost]
		public async Task<IActionResult> AddRating([FromBody] CourseRatingDto dto)
		{
			// Get UserId from Claims
			dto.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			if (string.IsNullOrEmpty(dto.UserId))
				return Unauthorized("Invalid Token");

			await _courseRatingService.AddCourseRatingAsync(dto);
			return Ok(new { Message = "Rating added successfully" });
		}

		[HttpGet("average/{courseId}")]
		public async Task<IActionResult> GetAverage(int courseId)
		{
			var avg = await _courseRatingService.GetCourseAverageRating(courseId);
			return Ok(new { CourseId = courseId, Average = avg });
		}

		[HttpGet("course/{courseId}")]
		public async Task<IActionResult> GetCourseRatings(int courseId)
		{
			var ratings = await _courseRatingService.GetRatingsForCourse(courseId);
			return Ok(ratings);
		}
	}
}
