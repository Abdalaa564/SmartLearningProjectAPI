using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLearning.Application.DTOs.QuizDto
{
	public class UpdateQuizDto
    {
		[Required]
		public int Quiz_Id { get; set; }
		[Required, MaxLength(100)]
		public string Quiz_Name { get; set; } = string.Empty;
		[Required]
		public int TotalMarks { get; set; }
		[Required]
		public int Duration { get; set; }
	}
}
