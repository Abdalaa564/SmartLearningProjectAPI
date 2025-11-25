using SmartLearning.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLearning.Application.DTOs.QuizDto
{
   public class QuestionDto
    {
		public int Question_Id { get; set; }
		public string Question_Text { get; set; } = string.Empty;
		public QuestionType Question_Type { get; set; }
		public int Grade_Point { get; set; }
		public List<ChoiceDto> Choices { get; set; } = new List<ChoiceDto>();
	}
}
