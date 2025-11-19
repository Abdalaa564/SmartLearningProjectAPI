namespace SmartLearning.Core.Model
{
    public class Choice
    {
        public int ChoiceId { get; set; }

        public int QuestionId { get; set; }

        public string ChoiceText { get; set; } = string.Empty;

        public bool IsCorrect { get; set; }


        public Questions Question { get; set; } = null!;
    }
}