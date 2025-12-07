using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLearning.Application.DTOs.EnrollmentDto
{
    public class EnrollmentInitiationResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int? EnrollmentId { get; set; }
        public string? TransactionId { get; set; }

   
        // Paymob iframe URL - frontend displays this

        public string? PaymentUrl { get; set; }
    }
}
