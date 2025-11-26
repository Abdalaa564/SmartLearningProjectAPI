using SmartLearning.Application.DTOs.RatingDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLearning.Application.Services
{
	public class RatingService : IRatingService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public RatingService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<RatingDto?> GetUserRatingForLessonAsync(int lessonId, string userId)
		{
			var ratingRepo = _unitOfWork.Repository<Rating>();

			var ratings = await ratingRepo.FindAsync(r =>
				r.Lesson_Id == lessonId && r.User_Id == userId);

			var rating = ratings.FirstOrDefault();
			if (rating == null) return null;

			return _mapper.Map<RatingDto>(rating);
		}

		public async Task<IReadOnlyList<RatingDto>> GetRatingsForLessonAsync(int lessonId)
		{
			var ratingRepo = _unitOfWork.Repository<Rating>();

			var ratings = await ratingRepo.FindAsync(r => r.Lesson_Id == lessonId);

			return _mapper.Map<IReadOnlyList<RatingDto>>(ratings);
		}

		public async Task<double?> GetAverageRatingForLessonAsync(int lessonId)
		{
			var ratingRepo = _unitOfWork.Repository<Rating>();

			var ratings = await ratingRepo.FindAsync(r => r.Lesson_Id == lessonId);

			if (!ratings.Any())
				return null;

			return ratings.Average(r => r.RatingValue);
		}

		public async Task<RatingDto> CreateOrUpdateRatingAsync(string userId, CreateOrUpdateRatingDto dto)
		{
			var ratingRepo = _unitOfWork.Repository<Rating>();

			var ratings = await ratingRepo.FindAsync(r =>
				r.Lesson_Id == dto.Lesson_Id && r.User_Id == userId);

			var existing = ratings.FirstOrDefault();

			if (existing == null)
			{
				
				var newRating = _mapper.Map<Rating>(dto);
				newRating.User_Id = userId; 

				await ratingRepo.AddAsync(newRating);
				await _unitOfWork.CompleteAsync();

				return _mapper.Map<RatingDto>(newRating);
			}
			else
			{
				
				_mapper.Map(dto, existing);
				

				ratingRepo.Update(existing);
				await _unitOfWork.CompleteAsync();

				return _mapper.Map<RatingDto>(existing);
			}
		}

		public async Task<bool> DeleteRatingAsync(int lessonId, string userId)
		{
			var ratingRepo = _unitOfWork.Repository<Rating>();

			var ratings = await ratingRepo.FindAsync(r =>
				r.Lesson_Id == lessonId && r.User_Id == userId);

			var existing = ratings.FirstOrDefault();
			if (existing == null) return false;

			ratingRepo.Remove(existing);
			await _unitOfWork.CompleteAsync();
			return true;
		}
	}
}
