
namespace SmartLearning.Core.Model
{
    public class Attendance
    {
        [Key]
        public int Attendance_Id { get; set; }
        public DateOnly Attendance_Date { get; set; }     // DateTime
        public DateTime CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public int Lesson_Id { get; set; }
        public Lessons Lesson { get; set; } = null!;

        [Required]
        public int StudentId { get; set; }
        public Student Student { get; set; } = null!;

        //public ICollection<Lessons> Lessons { get; set; } = new List<Lessons>();
    }
}
