using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLearning.Application.DTOs.RatingDto
{
   public class CreateOrUpdateRatingDto
	{
		[Required]
		public int Lesson_Id { get; set; }

		[Range(1, 5, ErrorMessage = "RatingValue must be between 1 and 5.")]
		public int RatingValue { get; set; }

		[MaxLength(1000)]
		public string Feedback { get; set; } = string.Empty;
	}
}
