using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLearning.Application.DTOs.QuizDto
{
   public class StudentAnswerResultDto
    {
		public int Question_Id { get; set; }
		public string Question_Text { get; set; } = string.Empty;
		public int Choice_Id { get; set; }
		public string Choice_Text { get; set; } = string.Empty;
		public bool Is_Correct { get; set; }
		public int Grade_Point { get; set; }
		public string CorrectAnswer { get; set; } = string.Empty;
	}
}
