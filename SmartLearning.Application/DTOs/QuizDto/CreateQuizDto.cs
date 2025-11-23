using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLearning.Application.DTOs.QuizDto
{
   public class CreateQuizDto
    {
		[Required, MaxLength(100)]
		public string Quiz_Name { get; set; } = string.Empty;
		[Required]
		public int Lesson_Id { get; set; }
		[Required]
		public int TotalMarks { get; set; }
		[Required]
		public int Duration { get; set; }
	}
}
