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
        //Task<PaymentResult> ProcessPaymentAsync(
        //    PaymentInfoDto payment,
        //    string transactionId,
        //    string courseName);

        Task<PaymentResult> ProcessPaymentAsync(PaymentInfoDto payment, string transactionId, string courseName);
        Task<string> InitiatePaymobPaymentAsync(PaymentInfoDto payment, string transactionId, string courseName, string studentEmail, string studentPhone, string studentName);
        Task<PaymentResult> VerifyPaymobCallbackAsync(Dictionary<string, string> callbackData);
    }

}
