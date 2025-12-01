namespace SmartLearning.Core.Model
{
    public class StudentAnswer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string User_Id { get; set; } = string.Empty;

        [Required]
        public int Quiz_Id { get; set; }

        [Required]
        public int Choice_Id { get; set; }
        public bool Is_Correct { get; set; }
        
        public int Question_Id { get; set; }
        public Questions Questions { get; set; } = null!;

        public ApplicationUser User { get; set; } = null!;
        public Quiz Quiz { get; set; } = null!;
        public Choice Choice { get; set; } = null!;
    }
}