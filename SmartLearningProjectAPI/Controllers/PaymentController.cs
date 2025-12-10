using AutoMapper;

namespace SmartLearningProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<PaymentController> _logger;
        public PaymentController(IPaymentService paymentService, IUnitOfWork unitOfWork, IMapper mapper, ILogger<PaymentController> logger)
        {
            _paymentService = paymentService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPayments()
        {
            _logger.LogInformation("Retrieving all payments...");

            var payments = await _unitOfWork.Repository<Payment>()
                    .GetAllAsync();

                var paymentDtos = _mapper.Map<List<PaymentResponseDto>>(payments);


                return Ok(new
                {
                    success = true,
                    count = paymentDtos.Count,
                    payments = paymentDtos
                });
            
            
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentById(int id)
        {
            _logger.LogInformation("Retrieving payment with ID {PaymentId}", id);

            var payment = await _unitOfWork.Repository<Payment>()
                    .GetByIdAsync(id);

                if (payment == null)
                {
                _logger.LogWarning("Payment with ID {PaymentId} not found", id);
                return NotFound(new { message = "Payment not found" });
                }

                var paymentDto = _mapper.Map<PaymentResponseDto>(payment);

                return Ok(new
                {
                    success = true,
                    payment = paymentDto
                });
           
        }

        /// <summary>
        /// Get payments by student ID
        /// GET /api/payment/student/{studentId}
        /// </summary>
        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetPaymentsByStudent(int studentId)
        {
            _logger.LogInformation("Retrieving payments for student {StudentId}", studentId);

            // Get all enrollments for this student
            var enrollments = await _unitOfWork.Repository<Enrollment>()
                    .FindAsync(e => e.StudentId == studentId);

                var enrollmentIds = enrollments.Select(e => e.Enroll_Id).ToList();

                // Get all payments for these enrollments
                var allPayments = await _unitOfWork.Repository<Payment>()
                    .GetAllAsync();

                var studentPayments = allPayments
                    .Where(p => enrollmentIds.Contains(p.Enroll_Id))
                    .ToList();
                var paymentDtos = _mapper.Map<List<PaymentResponseDto>>(studentPayments);

                return Ok(new
                {
                    success = true,
                    studentId,
                    count = paymentDtos.Count,
                    paymentDtos
                });
           
        }


    }
}
