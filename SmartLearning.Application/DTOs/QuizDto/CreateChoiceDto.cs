using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLearning.Application.DTOs.QuizDto
{
  public  class CreateChoiceDto
    {
		[Required, MaxLength(500)]
		public string ChoiceText { get; set; } = string.Empty;
		[Required]
		public bool IsCorrect { get; set; }
	}
}
