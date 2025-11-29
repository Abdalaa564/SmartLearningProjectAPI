


namespace SmartLearning.Application.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EnrollmentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // ------------------ Enroll Student ------------------
        public async Task<EnrollmentResponseDto> EnrollAsync(EnrollmentRequestDto request)
        {
            var courseRepo = _unitOfWork.Repository<Course>();
            var studentRepo = _unitOfWork.Repository<Student>();
            var enrollmentRepo = _unitOfWork.Repository<Enrollment>();

            // 1) تأكد إن الطالب موجود
            var student = await studentRepo.GetByIdAsync(request.StudentId);
            if (student == null)
            {
                return new EnrollmentResponseDto
                {
                    Success = false,
                    Message = "Student not found"
                };
            }

            // 2) تأكد إن الكورس موجود
            var course = await courseRepo.GetByIdAsync(request.CourseId);
            if (course == null)
            {
                return new EnrollmentResponseDto
                {
                    Success = false,
                    Message = "Course not found"
                };
            }

            // 3) تأكد إنه مش مسجل قبل كده في نفس الكورس
            var exists = await enrollmentRepo.FindAsync(
                e => e.Crs_Id == request.CourseId && e.StudentId == request.StudentId
            );

            if (exists.Any())
            {
                return new EnrollmentResponseDto
                {
                    Success = false,
                    Message = "Student is already enrolled in this course"
                };
            }

            // 4) السعر من الكورس
            var paidAmount = course.Price;
            var transactionId = request.TransactionId ?? Guid.NewGuid().ToString("N");

            // 5) إنشاء Enrollment
            var enrollment = new Enrollment
            {
                StudentId = request.StudentId,
                Crs_Id = request.CourseId,
                Enroll_Date = DateTime.UtcNow,
                Paid_Amount = paidAmount
            };

            await enrollmentRepo.AddAsync(enrollment);
            await _unitOfWork.CompleteAsync();

            // 6) نرجع Response
            return new EnrollmentResponseDto
            {
                Success = true,
                Message = "Enrollment completed successfully",
                EnrollmentId = enrollment.Enroll_Id,
                EnrollmentDate = enrollment.Enroll_Date,
                PaidAmount = paidAmount,
                TransactionId = transactionId
            };
        }

        // ------------------ UnEnroll Student ------------------
        public async Task<bool> UnenrollAsync(int studentId, int courseId)
        {
            var repo = _unitOfWork.Repository<Enrollment>();

            var enrollments = await repo.FindAsync(
                e => e.StudentId == studentId && e.Crs_Id == courseId
            );

            var enrollment = enrollments.FirstOrDefault();
            if (enrollment == null)
                return false;

            repo.Remove(enrollment);
            return await _unitOfWork.CompleteAsync() > 0;
        }

        // ------------------ Get Courses for Student ------------------
        public async Task<IEnumerable<CourseResponseDto>> GetStudentCoursesAsync(int studentId)
        {
            var enrollmentRepo = _unitOfWork.Repository<Enrollment>();

            var enrollments = await enrollmentRepo.FindAsync(
                e => e.StudentId == studentId,
                q => q.Include(e => e.Course)
                      .ThenInclude(c => c.Instructor)
                          .ThenInclude(i => i.User)
            );

            var courses = enrollments
                .Select(e => e.Course)
                .Distinct()
                .ToList();

            return _mapper.Map<IEnumerable<CourseResponseDto>>(courses);
        }
        public async Task<int> GetEnrollmentCountForCourseAsync(int courseId)
        {
            var repo = _unitOfWork.Repository<Enrollment>();
            var enrollments = await repo.FindAsync(e => e.Crs_Id == courseId);
            return enrollments.Count;
        }


    }
}