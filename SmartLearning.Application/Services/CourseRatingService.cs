using SmartLearning.Application.DTOs.Rating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLearning.Application.Services
{
   public class CourseRatingService : ICourseRatingService
	{
		private readonly IUnitOfWork _uow;
		private readonly IMapper _mapper;

		public CourseRatingService(IUnitOfWork uow, IMapper mapper)
		{
			_uow = uow;
			_mapper = mapper;
		}

		public async Task AddCourseRatingAsync(CourseRatingDto dto)
		{
			var exists = await _uow.Repository<CourseRating>()
				.FindAsync(r => r.CourseId == dto.CourseId && r.UserId == dto.UserId);

			if (exists.Any())
				throw new Exception("You already rated this course.");

			var rating = _mapper.Map<CourseRating>(dto);

			await _uow.Repository<CourseRating>().AddAsync(rating);
			await _uow.CompleteAsync();
		}

		public async Task<double> GetCourseAverageRating(int courseId)
		{
			var ratings = await _uow.Repository<CourseRating>()
				.FindAsync(r => r.CourseId == courseId);

			if (!ratings.Any()) return 0;

			return ratings.Average(r => r.RatingValue);
		}

		public async Task<IEnumerable<CourseRating>> GetRatingsForCourse(int courseId)
		{
			return await _uow.Repository<CourseRating>()
				.FindAsync(r => r.CourseId == courseId, q => q.Include(r => r.User));
		}
	}
}
