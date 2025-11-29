

using SmartLearning.Application.DTOs.EnrollmentDto;
using SmartLearning.Application.Interfaces;

namespace SmartLearning.Application.Services
{
    public class EnrollmentService : IEnrollmentService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPaymentService _paymentService;

        public EnrollmentService(IUnitOfWork unitOfWork, IMapper mapper, IPaymentService paymentService  )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _paymentService = paymentService;
        }
        public async Task<EnrollmentResponseDto> EnrollStudentAsync(EnrollmentRequestDto request)
        {

            // 1. Validate user exists
            var user = (await _unitOfWork.Repository<ApplicationUser>()
                    .FindAsync(u => u.Id == request.StudentId))
                    .FirstOrDefault();


            if (user == null)
                {
                    return new EnrollmentResponseDto
                    {
                        Success = false,
                        Message = "User not found"
                    };
                }

                // 2. Validate course exists
                var course = await _unitOfWork.Repository<Course>()
                    .GetByIdAsync(request.CourseId);

                if (course == null)
                {
                    return new EnrollmentResponseDto
                    {
                        Success = false,
                        Message = "Course not found"
                    };
                }

                // 3. Check if already enrolled
                var existingEnrollment = await _unitOfWork.Repository<Enrollment>()
                    .FindAsync(e => e.User_Id == request.StudentId && e.Crs_Id == request.CourseId);

                if (existingEnrollment.Any())
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
                var enrollment = new Enrollment
                {
                    User_Id = request.StudentId,
                    Crs_Id = request.CourseId,
                    Enroll_Date = DateTime.UtcNow,
                    Paid_Amount = request.Payment.Amount
                };

                await _unitOfWork.Repository<Enrollment>().AddAsync(enrollment);

                // 8. Create payment record
                var payment = new Payment
                {
                    Enroll_Id = enrollment.Enroll_Id,
                    Amount = request.Payment.Amount,
                    Payment_Method = request.Payment.PaymentMethod,
                    Transaction_Id = transactionId,
                    Payment_Date = DateTime.UtcNow,
                    Status = "Completed",
                    Gateway_Response = paymentResult.GatewayResponse
                };

                await _unitOfWork.Repository<Payment>().AddAsync(payment);

                // 9. Commit transaction
                await _unitOfWork.CompleteAsync();

                //_logger.LogInformation(
                //    "User {UserId} successfully enrolled in course {CourseId}. Transaction: {TransactionId}",
                //    request.UserId, request.CourseId, transactionId);

                return new EnrollmentResponseDto
                {
                    Success = true,
                    Message = "Enrollment successful! Welcome to the course.",
                    EnrollmentId = enrollment.Enroll_Id,
                    EnrollmentDate = enrollment.Enroll_Date,
                    TransactionId = transactionId,
                    PaidAmount = enrollment.Paid_Amount
                };
       
        }

        public async Task<List<EnrollmentDetailsDto>> GetCourseEnrollmentsAsync(int courseId)
        {
            var enrollments = await _unitOfWork.Repository<Enrollment>()
               .FindAsync(
                    e => e.Crs_Id == courseId,
                    e => e.User,
                    e => e.Course,
                    e => e.Payments
                );


            var enrollmentDtos = new List<EnrollmentDetailsDto>();

            foreach (var enrollment in enrollments.OrderByDescending(e => e.Enroll_Date))
            {
                var latestPayment = enrollment.Payments?.OrderByDescending(p => p.Payment_Date).FirstOrDefault();

                enrollmentDtos.Add(new EnrollmentDetailsDto
                {
                    EnrollId = enrollment.Enroll_Id,
                    UserId = enrollment.User_Id,
                    StudentName = enrollment.User?.FullName ?? enrollment.User?.UserName ?? "Unknown",
                    StudentEmail = enrollment.User?.Email ?? string.Empty,
                    StudentPhone = enrollment.User?.PhoneNumber,
                    CourseId = enrollment.Crs_Id,
                    CourseName = enrollment.Course?.Crs_Name ?? string.Empty,
                    CoursePrice = enrollment.Course?.Price ?? 0,
                    EnrollDate = enrollment.Enroll_Date,
                    PaidAmount = enrollment.Paid_Amount,
                    PaymentStatus = latestPayment?.Status,
                    TransactionId = latestPayment?.Transaction_Id,
                    PaymentDate = latestPayment?.Payment_Date
                });
            }

            return enrollmentDtos;
        }

        public async Task<EnrollmentDetailsDto?> GetEnrollmentByIdAsync(int enrollId)
        {
            var enrollments = await _unitOfWork.Repository<Enrollment>()
                 .FindAsync(
                     e => e.Enroll_Id == enrollId,
                     e => e.User,
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
                UserId = enrollment.User_Id,
                StudentName = enrollment.User?.FullName ?? enrollment.User?.UserName ?? "Unknown",
                StudentEmail = enrollment.User?.Email ?? string.Empty,
                StudentPhone = enrollment.User?.PhoneNumber,
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

        public async Task<List<EnrollmentDetailsDto>> GetUserEnrollmentsAsync(string userId)
        {
            var enrollments = await _unitOfWork.Repository<Enrollment>()
                 .FindAsync(
                     e => e.User_Id == userId,
                      e => e.User,
                    e => e.Course,
                    e => e.Payments
                 );

            var enrollmentDtos = new List<EnrollmentDetailsDto>();

            foreach (var enrollment in enrollments.OrderByDescending(e => e.Enroll_Date))
            {
                var latestPayment = enrollment.Payments?.OrderByDescending(p => p.Payment_Date).FirstOrDefault();

                enrollmentDtos.Add(new EnrollmentDetailsDto
                {
                    EnrollId = enrollment.Enroll_Id,
                    UserId = enrollment.User_Id,
                    StudentName = enrollment.User?.FullName ?? enrollment.User?.UserName ?? "Unknown",
                    StudentEmail = enrollment.User?.Email ?? string.Empty,
                    StudentPhone = enrollment.User?.PhoneNumber,
                    CourseId = enrollment.Crs_Id,
                    CourseName = enrollment.Course?.Crs_Name ?? string.Empty,
                    CoursePrice = enrollment.Course?.Price ?? 0,
                    EnrollDate = enrollment.Enroll_Date,
                    PaidAmount = enrollment.Paid_Amount,
                    PaymentStatus = latestPayment?.Status,
                    TransactionId = latestPayment?.Transaction_Id,
                    PaymentDate = latestPayment?.Payment_Date
                });
            }

            return enrollmentDtos;
        }

        public async Task<bool> IsStudentEnrolledAsync(string userId, int courseId)
        {
            var enrollments = await _unitOfWork.Repository<Enrollment>()
                 .FindAsync(e => e.User_Id == userId && e.Crs_Id == courseId);

            return enrollments.Any();
        }
        private string GenerateTransactionId()
        {
            return $"TXN_{DateTime.UtcNow:yyyyMMddHHmmss}_{Guid.NewGuid().ToString("N")[..8].ToUpper()}";
        }
    }
}
