namespace SmartLearning.Core.Model
{
    public class InstructorCourse
    {
        public int InstructorId { get; set; }
        public Instructor Instructor { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public DateTime AssignedDate { get; set; }

        public int WeeklyHours { get; set; }

        public bool IsActive { get; set; }
    }
}