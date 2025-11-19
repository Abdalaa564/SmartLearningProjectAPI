namespace SmartLearning.Core.Model
{
    public class Choice
    {
        [Key]
        public int ChoiceId { get; set; }

        [Required]
        public int QuestionId { get; set; }

        [Required, MaxLength(500)]
        public string ChoiceText { get; set; } = string.Empty;

        public bool IsCorrect { get; set; }


        public Questions Question { get; set; } = null!;
    }
}