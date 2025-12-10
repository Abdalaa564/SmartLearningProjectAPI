using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLearning.Application.DTOs.Rating
{
   public class CourseRatingDto
    {
		public int CourseId { get; set; }
		public string UserId { get; set; } = string.Empty;
		public int RatingValue { get; set; }
		public string Feedback { get; set; } = string.Empty;
	}
}
