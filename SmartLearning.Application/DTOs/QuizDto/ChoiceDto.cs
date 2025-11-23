using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLearning.Application.DTOs.QuizDto
{
  public  class ChoiceDto
    {
		public int ChoiceId { get; set; }
		public string ChoiceText { get; set; } = string.Empty;
		public bool IsCorrect { get; set; }
	}
}
