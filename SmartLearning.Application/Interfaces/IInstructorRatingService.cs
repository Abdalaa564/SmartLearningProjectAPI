using SmartLearning.Application.DTOs.Rating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLearning.Application.Interfaces
{
   public interface IInstructorRatingService
    {
		Task AddInstructorRatingAsync(InstructorRatingDto dto);
		Task<double> GetInstructorAverageRating(int instructorId);
		Task<IEnumerable<InstructorRating>> GetRatingsForInstructor(int instructorId);
		Task<bool> HasUserRatedInstructor(int instructorId, string userId);

	}
}
