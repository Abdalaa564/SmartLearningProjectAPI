using SmartLearning.Application.DTOs.RatingDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLearning.Application.Interfaces
{
   public interface IRatingService
    {
		Task<RatingDto?> GetUserRatingForLessonAsync(int lessonId, string userId);
		Task<IReadOnlyList<RatingDto>> GetRatingsForLessonAsync(int lessonId);
		Task<double?> GetAverageRatingForLessonAsync(int lessonId);
		Task<RatingDto> CreateOrUpdateRatingAsync(string userId, CreateOrUpdateRatingDto dto);
		Task<bool> DeleteRatingAsync(int lessonId, string userId);
	}
}
