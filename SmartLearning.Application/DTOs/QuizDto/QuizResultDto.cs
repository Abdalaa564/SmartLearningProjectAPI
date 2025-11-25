using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLearning.Application.DTOs.QuizDto
{
   public class QuizResultDto
    {
		public int Quiz_Id { get; set; }
		public string Quiz_Name { get; set; } = string.Empty;
		public int TotalMarks { get; set; }
		public int ObtainedMarks { get; set; }
		public double Percentage { get; set; }
		public int CorrectAnswers { get; set; }
		public int TotalQuestions { get; set; }
		public List<StudentAnswerResultDto> Answers { get; set; } = new List<StudentAnswerResultDto>();
	}
}
