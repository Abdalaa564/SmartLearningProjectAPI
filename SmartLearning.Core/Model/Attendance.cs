

namespace SmartLearning.Core.Model
{
    public class Attendance
    {
        public int Attendance_Id { get; set; }
        public DateOnly Attendance_Date { get; set; }     // DateTime
        public DateTime CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public string User_Id { get; set; } = string.Empty;

        public ApplicationUser User { get; set; } = null!;

        public ICollection<Lessons> Lessons { get; set; } = new List<Lessons>();
    }
}
