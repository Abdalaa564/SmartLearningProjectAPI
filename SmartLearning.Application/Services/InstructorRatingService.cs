using SmartLearning.Application.DTOs.Rating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLearning.Application.Services
{
   public class InstructorRatingService : IInstructorRatingService
	{
		private readonly IUnitOfWork _uow;
		private readonly IMapper _mapper;

		public InstructorRatingService(IUnitOfWork uow, IMapper mapper)
		{
			_uow = uow;
			_mapper = mapper;
		}

		public async Task AddInstructorRatingAsync(InstructorRatingDto dto)
		{
			var exists = await _uow.Repository<InstructorRating>()
				.FindAsync(r => r.InstructorId == dto.InstructorId && r.UserId == dto.UserId);

			if (exists.Any())
				throw new Exception("You already rated this instructor.");

			var rating = _mapper.Map<InstructorRating>(dto);

			await _uow.Repository<InstructorRating>().AddAsync(rating);
			await _uow.CompleteAsync();
		}

		public async Task<double> GetInstructorAverageRating(int instructorId)
		{
			var ratings = await _uow.Repository<InstructorRating>()
				.FindAsync(r => r.InstructorId == instructorId);

			if (!ratings.Any()) return 0;

			return ratings.Average(r => r.RatingValue);
		}

		public async Task<IEnumerable<InstructorRating>> GetRatingsForInstructor(int instructorId)
		{
			return await _uow.Repository<InstructorRating>()
				.FindAsync(r => r.InstructorId == instructorId, q => q.Include(r => r.User));
		}
		public async Task<bool> HasUserRatedInstructor(int instructorId, string userId)
		{
			var exists = await _uow.Repository<InstructorRating>()
				.FindAsync(r => r.InstructorId == instructorId && r.UserId == userId);

			return exists.Any();
		}
	}
}
