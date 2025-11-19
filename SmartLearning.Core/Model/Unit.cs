namespace SmartLearning.Core.Model
{
    public class Unit
    {
        [Key]
        public int Unit_Id { get; set; }

        [Required]
        public int Crs_Id { get; set; }

        [Required, MaxLength(100)]
        public string Unit_Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Unit_Description { get; set; } = string.Empty;
        public int OrderIndex { get; set; }

        //(M → 1)
        public Course Course { get; set; } = null!;
        public ICollection<Lessons> Lessons { get; set; } = new List<Lessons>();
    }
}