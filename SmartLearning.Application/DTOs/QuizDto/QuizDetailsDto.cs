using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLearning.Application.DTOs.QuizDto
{
	public class QuizDetailsDto
    {
		public int Quiz_Id { get; set; }
		public string Quiz_Name { get; set; } = string.Empty;
		public int TotalMarks { get; set; }
		public int Duration { get; set; }
		public int Lesson_Id { get; set; }
		public string Lesson_Name { get; set; } = string.Empty;
		public List<QuestionDto> Questions { get; set; } = new List<QuestionDto>();
	}
}
