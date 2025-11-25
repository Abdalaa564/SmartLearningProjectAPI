using SmartLearning.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLearning.Application.DTOs.QuizDto
{
   public class CreateQuestionDto
    {
		[Required]
		public int Quiz_Id { get; set; }
		[Required, MaxLength(500)]
		public string Question_Text { get; set; } = string.Empty;
		[Required]
		public QuestionType Question_Type { get; set; }
		[Required]
		public int Grade_Point { get; set; }
		[Required, MaxLength(500)]
		//public string CorrectAnswer { get; set; } = string.Empty;
		public List<CreateChoiceDto> Choices { get; set; } = new List<CreateChoiceDto>();
	}
}
