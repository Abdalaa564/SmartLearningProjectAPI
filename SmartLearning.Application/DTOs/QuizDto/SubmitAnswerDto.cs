using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLearning.Application.DTOs.QuizDto
{
   public class SubmitAnswerDto
    {
		[Required]
		public int Quiz_Id { get; set; }
		[Required]
		public int Question_Id { get; set; }
		[Required]
		public int Choice_Id { get; set; }
	}
}
