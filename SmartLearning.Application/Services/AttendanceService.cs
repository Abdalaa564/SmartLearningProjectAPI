using SmartLearning.Application.DTOs.Attendance;

namespace SmartLearning.Application.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AttendanceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // ✅ 1) الطالب يعمل CheckIn (حضور)
        public async Task<bool> CheckInAsync(int lessonId, string userId)
        {
            var studentRepo = _unitOfWork.Repository<Student>();
            var attendanceRepo = _unitOfWork.Repository<Attendance>();
            var lessonRepo = _unitOfWork.Repository<Lessons>();

            // 1) نتأكد إن فيه Student مرتبط بالـ userId
            var students = await studentRepo.FindAsync(s => s.UserId == userId);
            var student = students.FirstOrDefault();
            if (student == null)
                return false;  // أو ترمي Exception على حسب ستايلك

            // 2) نتأكد إن الـ lesson موجود
            var lesson = await lessonRepo.GetByIdAsync(lessonId);
            if (lesson == null)
                return false;

            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            // 3) نتأكد إنه مش مسجل حضور لنفس الدرس اليوم
            var existing = await attendanceRepo.FindAsync(a =>
                a.Lesson_Id == lessonId &&
                a.StudentId == student.Id &&
                a.Attendance_Date == today
            );

            if (existing.Any())
                return false;  // Already checked-in today

            var attendance = new Attendance
            {
                Lesson_Id = lessonId,
                StudentId = student.Id,
                Attendance_Date = today,
                CheckIn = DateTime.UtcNow
            };

            await attendanceRepo.AddAsync(attendance);
            return await _unitOfWork.CompleteAsync() > 0;
        }

        // ✅ 2) CheckOut
        public async Task<bool> CheckOutAsync(int lessonId, string userId)
        {
            var studentRepo = _unitOfWork.Repository<Student>();
            var attendanceRepo = _unitOfWork.Repository<Attendance>();

            var students = await studentRepo.FindAsync(s => s.UserId == userId);
            var student = students.FirstOrDefault();
            if (student == null) return false;

            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            var attendances = await attendanceRepo.FindAsync(a =>
                a.Lesson_Id == lessonId &&
                a.StudentId == student.Id &&
                a.Attendance_Date == today
            );

            var attendance = attendances.FirstOrDefault();
            if (attendance == null)
                return false;

            attendance.CheckOut = DateTime.UtcNow;
            attendanceRepo.Update(attendance);
            return await _unitOfWork.CompleteAsync() > 0;
        }

        // ✅ 3) الإنستراكتور يشوف الطلاب اللي حضروا درس معيّن
        public async Task<IReadOnlyList<LessonAttendanceStudentDto>> GetLessonAttendanceAsync(
            int lessonId,
            string instructorUserId)
        {
            var attendanceRepo = _unitOfWork.Repository<Attendance>();

            var attendances = await attendanceRepo.FindAsync(
                a => a.Lesson_Id == lessonId,
                q => q
                    .Include(a => a.Student)
                        .ThenInclude(s => s.User) // عشان نجيب الإيميل من ApplicationUser
                    .Include(a => a.Lesson)
                        .ThenInclude(l => l.Unit)
                            .ThenInclude(u => u.Course)
                                .ThenInclude(c => c.Instructor)
            );

            // 1) نتأكد إن الدرس ده فعلا تابع للإنستراكتور اللي لوجدين
            var sample = attendances.FirstOrDefault();
            if (sample == null)
                return Array.Empty<LessonAttendanceStudentDto>();

            var courseInstructorUserId = sample.Lesson.Unit.Course.Instructor.UserId;
            if (!string.Equals(courseInstructorUserId, instructorUserId, StringComparison.OrdinalIgnoreCase))
            {
                // ممكن ترجع Empty أو ترمي UnauthorizedException على حسب ما متعود
                return Array.Empty<LessonAttendanceStudentDto>();
            }

            // 2) نرجّع List بالطلاب
            var result = attendances
                .Select(a => new LessonAttendanceStudentDto
                {
                    StudentId = a.Student.Id,
                    FullName = $"{a.Student.FirstName} {a.Student.LastName}",
                    Email = a.Student.User.Email,
                    AttendanceDate = a.Attendance_Date,
                    CheckIn = a.CheckIn,
                    CheckOut = a.CheckOut
                })
                .OrderBy(a => a.FullName)
                .ToList();

            return result;
        }
    }
}