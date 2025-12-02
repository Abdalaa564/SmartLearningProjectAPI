


using SmartLearning.Application.DTOs.EnrollmentDto;

namespace SmartLearning.Application.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPaymentService _paymentService;

        public EnrollmentService(IUnitOfWork unitOfWork, IMapper mapper, IPaymentService paymentService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _paymentService = paymentService;
        }

        // ------------------ Enroll Student ------------------
        public async Task<EnrollmentResponseDto> EnrollStudentAsync(EnrollmentRequestDto request)
        {
            var courseRepo = _unitOfWork.Repository<Course>();
            var studentRepo = _unitOfWork.Repository<Student>();
            var enrollmentRepo = _unitOfWork.Repository<Enrollment>();

            
            var student = await studentRepo.GetByIdAsync(request.StudentId);
            if (student == null)
            {
                return new EnrollmentResponseDto
                {
                    Success = false,
                    Message = "Student not found"
                };
            }

          
            var course = await courseRepo.GetByIdAsync(request.CourseId);
            if (course == null)
            {
                return new EnrollmentResponseDto
                {
                    Success = false,
                    Message = "Course not found"
                };
            }

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

            // 4. Validate payment amount
            if (request.Payment.Amount != course.Price)
            {
                return new EnrollmentResponseDto
                {
                    Success = false,
                    Message = $"Payment amount ({request.Payment.Amount:C}) must match course price ({course.Price:C})"
                };
            }

            // 5. Generate transaction ID
            var transactionId = GenerateTransactionId();

            // 6. Process payment
            var paymentResult = await _paymentService.ProcessPaymentAsync(
                          request.Payment,
                         transactionId,
                         course.Crs_Name);


            if (!paymentResult.Success)
            {
                return new EnrollmentResponseDto
                {
                    Success = false,
                    Message = $"Payment failed: {paymentResult.Message}"
                };
            }

            // 7. Create enrollment
            var enrollment = _mapper.Map<Enrollment>(request);
            enrollment.StudentId = request.StudentId;
            enrollment.Crs_Id = request.CourseId;

            await enrollmentRepo.AddAsync(enrollment);
            await _unitOfWork.CompleteAsync();

            // 8. Create payment record
            var payment = _mapper.Map<Payment>(request.Payment);
             payment.Enroll_Id = enrollment.Enroll_Id;
             payment.Transaction_Id = transactionId;
             payment.Status = "Completed";
             payment.Gateway_Response = paymentResult.GatewayResponse;

            await _unitOfWork.Repository<Payment>().AddAsync(payment);
            await _unitOfWork.CompleteAsync();


            // 6) نرجع Response
            var response = _mapper.Map<EnrollmentResponseDto>(enrollment);
            response.Success = true;
            response.Message = "Enrollment completed successfully";
            response.TransactionId = transactionId;
            return response;

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
            return enrollments.Count();
        }

        public async Task<EnrollmentDetailsDto?> GetEnrollmentByIdAsync(int enrollId)
        {
            var enrollments = await _unitOfWork.Repository<Enrollment>()
                 .FindAsync(
                     e => e.Enroll_Id == enrollId,
                     e => e.Student,
                     e => e.Student.User,
                    e => e.Course,
                    e => e.Payments
                 );

            var enrollment = enrollments.FirstOrDefault();
            if (enrollment == null)
                return null;

            var latestPayment = enrollment.Payments?.OrderByDescending(p => p.Payment_Date).FirstOrDefault();

            return new EnrollmentDetailsDto
            {
                EnrollId = enrollment.Enroll_Id,
                UserId = enrollment.StudentId,
                StudentEmail = enrollment.Student?.User?.Email ?? string.Empty,
                StudentPhone = enrollment.Student?.User?.PhoneNumber,
                StudentName = enrollment.Student?.User?.UserName,
                CourseId = enrollment.Crs_Id,
                CourseName = enrollment.Course?.Crs_Name ?? string.Empty,
                CoursePrice = enrollment.Course?.Price ?? 0,
                EnrollDate = enrollment.Enroll_Date,
                PaidAmount = enrollment.Paid_Amount,
                PaymentStatus = latestPayment?.Status,
                TransactionId = latestPayment?.Transaction_Id,
                PaymentDate = latestPayment?.Payment_Date
            };
        }

        public async Task<bool> IsStudentEnrolledAsync(int userId, int courseId)
        {
            var enrollments = await _unitOfWork.Repository<Enrollment>()
                 .FindAsync(e => e.StudentId == userId && e.Crs_Id == courseId);

            return enrollments.Any();
        }
        private string GenerateTransactionId()
        {
            return $"TXN_{DateTime.UtcNow:yyyyMMddHHmmss}_{Guid.NewGuid().ToString("N")[..8].ToUpper()}";
        }

        public async Task<int> GetNotEnrolledCountForCourseAsync(int courseId)
        {
            var studentRepo = _unitOfWork.Repository<Student>();
            var enrollmentRepo = _unitOfWork.Repository<Enrollment>();

            // All Students
            var allStudents = await studentRepo.GetAllAsync();

            //all enroll =ments for the course
            var enrollments = await enrollmentRepo.FindAsync(e => e.Crs_Id == courseId);

            // IDs for all enrolled students
            var enrolledStudentIds = enrollments
                .Select(e => e.StudentId)
                .Distinct()
                .ToHashSet();

            //Number of students not enrolled in the course
            var notEnrolledCount = allStudents.Count(s => !enrolledStudentIds.Contains(s.Id));

            return notEnrolledCount;
        }
    }
}