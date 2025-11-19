namespace SmartLearning.Core.Model
{
    public class StudentAnswer
    {
        public int Id { get; set; }
        public string User_Id { get; set; } = string.Empty;
        public int Quiz_Id { get; set; }
        public int Choice_Id { get; set; }
        public bool Is_Correct { get; set; }

        public ApplicationUser User { get; set; } = null!;
        public Quiz Quiz { get; set; } = null!;
        public Choice Choice { get; set; } = null!;
    }
}