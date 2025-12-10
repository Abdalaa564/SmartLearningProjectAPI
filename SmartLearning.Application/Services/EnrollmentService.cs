


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
        public async Task<EnrollmentInitiationResponseDto> EnrollStudentAsync(EnrollmentRequestDto request)
        {

            var courseRepo = _unitOfWork.Repository<Course>();
            var studentRepo = _unitOfWork.Repository<Student>();
            var enrollmentRepo = _unitOfWork.Repository<Enrollment>();
            var paymentRepo = _unitOfWork.Repository<Payment>();


            var student = (await studentRepo.FindAsync(
                      s => s.Id == request.StudentId,
                    query => query.Include(s => s.User)
                    )).FirstOrDefault();
            if (student == null)
            {
                return new EnrollmentInitiationResponseDto
                {
                    Success = false,
                    Message = "Student not found"
                };
            }

          
            var course = await courseRepo.GetByIdAsync(request.CourseId);
            if (course == null)
            {
                return new EnrollmentInitiationResponseDto
                {
                    Success = false,
                    Message = "Course not found"
                };
            }

            // 3. Check if already enrolled with completed payment
            var existingEnrollments = await _unitOfWork.Repository<Enrollment>()
                .FindAsync(e => e.StudentId == request.StudentId && e.Crs_Id == request.CourseId);

            foreach (var existingEnroll in existingEnrollments)
            {
                var existingPayments = await _unitOfWork.Repository<Payment>()
                    .FindAsync(p => p.Enroll_Id == existingEnroll.Enroll_Id);

                if (existingPayments.Any(p => p.Status == "Completed"))
                {
                    return new EnrollmentInitiationResponseDto
                    {
                        Success = false,
                        Message = "Student is already enrolled in this course"
                    };
                }
            }

            // 4. Validate payment amount
            if (request.Payment.Amount != course.Price)
            {
                return new EnrollmentInitiationResponseDto
                {
                    Success = false,
                    Message = $"Payment amount ({request.Payment.Amount:C}) must match course price ({course.Price:C})"
                };
            }

            // 5. Generate transaction ID
            var transactionId = GenerateTransactionId();
       

            //  Create enrollment record
            var enrollment = _mapper.Map<Enrollment>(request);
            enrollment.StudentId = request.StudentId;
            enrollment.Crs_Id = request.CourseId;
            enrollment.Paid_Amount = request.Payment.Amount;

            await enrollmentRepo.AddAsync(enrollment);
            await _unitOfWork.CompleteAsync();



            //  Create payment record
            var payment = _mapper.Map<Payment>(request.Payment);
            payment.Enroll_Id = enrollment.Enroll_Id;
            payment.Transaction_Id = transactionId;
            payment.Status = "Pending";
            payment.Payment_Method = "Paymob";
            payment.Gateway_Response = null;

            await _unitOfWork.Repository<Payment>().AddAsync(payment);
            await _unitOfWork.CompleteAsync();

            //Paymob payment
            var paymentUrl = await _paymentService.InitiatePaymobPaymentAsync(
                   request.Payment,
                   transactionId,
                   course.Crs_Name,
                   student.User.Email,
                   student.PhoneNumber ?? "01000000000", // Fallback phone
                   $"{student.FirstName} {student.LastName}"
               );
           
            return new EnrollmentInitiationResponseDto
            {
                Success = true,
                Message = "Enrollment initiated. Please complete payment.",
                EnrollmentId = enrollment.Enroll_Id,
                TransactionId = transactionId,
                PaymentUrl = paymentUrl
            };

            // 6. Process payment
            //var paymentResult = await _paymentService.ProcessPaymentAsync(
            //              request.Payment,
            //             transactionId,
            //             course.Crs_Name);


            //if (!paymentResult.Success)
            //{
            //    return new EnrollmentResponseDto
            //    {
            //        Success = false,
            //        Message = $"Payment failed: {paymentResult.Message}"
            //    };
            //}

            // 7. Create enrollment
            //var enrollment = _mapper.Map<Enrollment>(request);
            //enrollment.StudentId = request.StudentId;
            //enrollment.Crs_Id = request.CourseId;

            //await enrollmentRepo.AddAsync(enrollment);
            //await _unitOfWork.CompleteAsync();



            //// 6) نرجع Response
            //var response = _mapper.Map<EnrollmentResponseDto>(enrollment);
            //response.Success = true;
            //response.Message = "Enrollment completed successfully";
            //response.TransactionId = transactionId;
            //return response;

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

         //   var latestPayment = enrollment.Payments?.OrderByDescending(p => p.Payment_Date).FirstOrDefault();

            return _mapper.Map<EnrollmentDetailsDto>(enrollment);
        }

        public async Task<bool> IsStudentEnrolledAsync(int userId, int courseId)
        {
            var enrollments = await _unitOfWork.Repository<Enrollment>()
        .FindAsync(e => e.StudentId == userId && e.Crs_Id == courseId,
            q => q.Include(e => e.Payments));

            var enrollment = enrollments.FirstOrDefault();

            if (enrollment == null)
                return false;

            // Must have completed payment
            return enrollment.Payments.Any(p => p.Status == "Completed");
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

        public async Task<EnrollmentResponseDto> CompleteEnrollmentFromCallbackAsync(Dictionary<string, string> callbackData)
        {
            // 1. Verify payment with Paymob
            var paymentResult = await _paymentService.VerifyPaymobCallbackAsync(callbackData);

            if (!paymentResult.Success)
            {
                return new EnrollmentResponseDto
                {
                    Success = false,
                    Message = $"Payment verification failed: {paymentResult.Message}"
                };
            }

            // 2. Get transaction ID from callback
            var transactionId = callbackData.GetValueOrDefault("merchant_order_id");
            if (string.IsNullOrEmpty(transactionId))
            {
                return new EnrollmentResponseDto
                {
                    Success = false,
                    Message = "Transaction ID not found in callback"
                };
            }

            // 3. Find the pending payment record
            var payments = await _unitOfWork.Repository<Payment>()
                .FindAsync(p => p.Transaction_Id == transactionId);
            var payment = payments.FirstOrDefault();

            if (payment == null)
            {
                return new EnrollmentResponseDto
                {
                    Success = false,
                    Message = "Payment record not found"
                };
            }

            // 4. Prevent duplicate processing
            if (payment.Status == "Completed")
            {
                var completedEnrollment = await _unitOfWork.Repository<Enrollment>()
                    .GetByIdAsync(payment.Enroll_Id);

                var completeResponse = _mapper.Map<EnrollmentResponseDto>(completedEnrollment);
                completeResponse.Success = true;
                completeResponse.Message = "Payment already processed";
                completeResponse.TransactionId = transactionId;
                return completeResponse;
                //return new EnrollmentResponseDto
                //{
                //    Success = true,
                //    Message = "Payment already processed",
                //    EnrollmentId = completedEnrollment.Enroll_Id,
                //    EnrollmentDate = completedEnrollment.Enroll_Date,
                //    TransactionId = transactionId,
                //    PaidAmount = payment.Amount
                //};
            }

            // 5. Update payment record to Completed
            payment.Status = "Completed";
            payment.Gateway_Response = paymentResult.GatewayResponse;
            payment.Payment_Date = DateTime.UtcNow;

            // 6. Get enrollment
            var enrollment = await _unitOfWork.Repository<Enrollment>()
                .GetByIdAsync(payment.Enroll_Id);

            // 7. Commit changes
            await _unitOfWork.CompleteAsync();
            var response = _mapper.Map<EnrollmentResponseDto>(enrollment);
            response.Success = true;
            response.Message = "Enrollment successful! Welcome to the course.";
            response.TransactionId = transactionId;
            return response;
            //return new EnrollmentResponseDto
            //{
            //    Success = true,
            //    Message = "Enrollment successful! Welcome to the course.",
            //    EnrollmentId = enrollment.Enroll_Id,
            //    EnrollmentDate = enrollment.Enroll_Date,
            //    TransactionId = transactionId,
            //    PaidAmount = payment.Amount
            //};
        }

        public async Task<EnrollmentStatusDto> GetEnrollmentStatusAsync(string transactionId)
        {
            var payments = await _unitOfWork.Repository<Payment>()
                .FindAsync(p => p.Transaction_Id == transactionId, 
                           p => p.Enrollment);
            var payment = payments.FirstOrDefault();

            if (payment == null)
            {
                return null;
            }

            //var enrollment = await _unitOfWork.Repository<Enrollment>()
            //    .GetByIdAsync(payment.Enroll_Id);

            return _mapper.Map<EnrollmentStatusDto>(payment);
        }

        public async Task<EnrollmentDetailsDto> GetEnrollmentDetailsAsync(int enrollmentId)
        {
            var enrollments = await _unitOfWork.Repository<Enrollment>()
        .FindAsync(
            e => e.Enroll_Id == enrollmentId,
            e => e.Student,
            e => e.Student.User,
            e => e.Course,
            e => e.Payments
        );

            var enrollment = enrollments.FirstOrDefault();
            if (enrollment == null)
                return null;

            return _mapper.Map<EnrollmentDetailsDto>(enrollment);
        }
    }
}