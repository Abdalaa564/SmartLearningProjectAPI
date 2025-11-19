namespace SmartLearning.Core.Model
{
    public class Grades
    {
        public int Id { get; set; }
        public string Std_Id { get; set; } = string.Empty;
        public int Quize_Id { get; set; }
        //public int Course_Id { get; set; }
        public decimal Value { get; set; }

        public ApplicationUser Student { get; set; } = null!;
        public Quiz Quiz { get; set; } = null!;
        //public Course Course { get; set; } = null!;
    }
}