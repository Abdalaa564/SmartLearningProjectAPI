
namespace SmartLearning.Application.DTOs.QuizDto
{
  public  class ChoiceDto
    {
		public int ChoiceId { get; set; }
		public string ChoiceText { get; set; } = string.Empty;
		public bool IsCorrect { get; set; }
	}
}
