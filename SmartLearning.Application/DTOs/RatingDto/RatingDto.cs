using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLearning.Application.DTOs.RatingDto
{
   public class RatingDto
    {
		public int Lesson_Id { get; set; }
		public string User_Id { get; set; } = string.Empty;
		public int RatingValue { get; set; }
		public string Feedback { get; set; } = string.Empty;
	}
}
