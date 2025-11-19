namespace SmartLearning.Core.Model
{
    public class Grades
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Std_Id { get; set; } = string.Empty;

        [Required]
        public int Quize_Id { get; set; }
        //public int Course_Id { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal Value { get; set; }

        public ApplicationUser Student { get; set; } = null!;
        public Quiz Quiz { get; set; } = null!;
        //public Course Course { get; set; } = null!;
    }
}