using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartLearning.Application.DTOs.RatingDto;

namespace SmartLearningProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
		private readonly IRatingService _ratingService;
		private readonly UserManager<ApplicationUser> _userManager;

		public RatingController(
			IRatingService ratingService,
			UserManager<ApplicationUser> userManager)
		{
			_ratingService = ratingService;
			_userManager = userManager;
		}

		// GET api/ratings/lesson/5
		[HttpGet("lesson/{lessonId:int}")]
		public async Task<IActionResult> GetLessonRatings(int lessonId)
		{
			var ratings = await _ratingService.GetRatingsForLessonAsync(lessonId);
			return Ok(ratings);
		}

		// GET api/ratings/lesson/5/average
		[HttpGet("lesson/{lessonId:int}/average")]
		public async Task<IActionResult> GetLessonAverageRating(int lessonId)
		{
			var average = await _ratingService.GetAverageRatingForLessonAsync(lessonId);
			var ratings = await _ratingService.GetRatingsForLessonAsync(lessonId);

			return Ok(new
			{
				Average = average,
				Count = ratings.Count
			});
		}

		// GET api/ratings/lesson/5/me
		[Authorize]
		[HttpGet("lesson/{lessonId:int}/me")]
		public async Task<IActionResult> GetMyRatingForLesson(int lessonId)
		{
			var userId = _userManager.GetUserId(User);
			if (userId == null) return Unauthorized();

			var rating = await _ratingService.GetUserRatingForLessonAsync(lessonId, userId);
			if (rating == null) return NotFound();

			return Ok(rating);
		}

		// POST api/ratings
		[Authorize]
		[HttpPost]
		public async Task<IActionResult> CreateOrUpdateRating([FromBody] CreateOrUpdateRatingDto dto)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			var userId = _userManager.GetUserId(User);
			if (userId == null) return Unauthorized();

			var result = await _ratingService.CreateOrUpdateRatingAsync(userId, dto);
			return Ok(result);
		}

		// DELETE api/ratings/lesson/5
		[Authorize]
		[HttpDelete("lesson/{lessonId:int}")]
		public async Task<IActionResult> DeleteMyRating(int lessonId)
		{
			var userId = _userManager.GetUserId(User);
			if (userId == null) return Unauthorized();

			var deleted = await _ratingService.DeleteRatingAsync(lessonId, userId);
			if (!deleted) return NotFound();

			return NoContent();
		}
	}
}
