
namespace SmartLearning.Core.Model
{
    public class Enrollment
    {
        public int Enroll_Id { get; set; }
        public string User_Id { get; set; } = string.Empty;
        public int Crs_Id { get; set; }
        public DateTime Enroll_Date { get; set; }
        public decimal Paid_Amount { get; set; }

        public ApplicationUser User { get; set; } = null!;
        public Course Course { get; set; } = new Course();
    }
}
