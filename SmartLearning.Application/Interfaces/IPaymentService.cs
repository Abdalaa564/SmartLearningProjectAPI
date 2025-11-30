using SmartLearning.Application.DTOs.PaymentDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLearning.Application.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentResult> ProcessPaymentAsync(
            PaymentInfoDto payment,
            string transactionId,
            string courseName);
    }

}
