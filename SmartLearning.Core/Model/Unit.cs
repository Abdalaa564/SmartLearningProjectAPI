namespace SmartLearning.Core.Model
{
    public class Unit
    {
        public int Unit_Id { get; set; }
        public int Crs_Id { get; set; }
        public string Unit_Name { get; set; } = string.Empty;
        public string Unit_Description { get; set; } = string.Empty;
        public int OrderIndex { get; set; }

        //(M → 1)
        public Course Course { get; set; } = null!;
        public ICollection<Lessons> Lessons { get; set; } = new List<Lessons>();
    }
}