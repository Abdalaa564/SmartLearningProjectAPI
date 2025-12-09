using SmartLearning.Application.DTOs.Rating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLearning.Application.Interfaces
{
   public interface ICourseRatingService
    {
		Task AddCourseRatingAsync(CourseRatingDto dto);
		Task<double> GetCourseAverageRating(int courseId);
		Task<IEnumerable<CourseRating>> GetRatingsForCourse(int courseId);
	}
}
