using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLearning.Application.DTOs.QuizDto
{
   public class StartQuizDto
    {
		public int Quiz_Id { get; set; }
		public string Quiz_Name { get; set; } = string.Empty;
		public int Duration { get; set; }
		public int TotalMarks { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public List<QuestionDto> Questions { get; set; } = new List<QuestionDto>();
	}
}
