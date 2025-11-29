

using SmartLearning.Application.DTOs.PaymentDto;
using System.Text.Json;

namespace SmartLearning.Application.Services
{
    public class PaymentService : IPaymentService
    {
      

        public async Task<PaymentResult> ProcessPaymentAsync(
            PaymentInfoDto payment,
            string transactionId,
            string courseName)
        {
            
                // 1. Basic validation
                if (string.IsNullOrWhiteSpace(payment.PaymentMethod))
                {
                    return PaymentResult.Failed("Payment method is required");
                }

                if (payment.Amount <= 0)
                {
                    return PaymentResult.Failed("Payment amount must be greater than zero");
                }

                // 2. Simulate third-party payment gateway
                await Task.Delay(500); // Mock external API call

                // 3. Simulate a gateway response payload
                var gatewayResponse = JsonSerializer.Serialize(new
                {
                    gateway = "Demo Gateway",
                    status = "approved",
                    transaction_id = transactionId,
                    timestamp = DateTime.UtcNow,
                    payment_method = payment.PaymentMethod,
                    amount = payment.Amount,
                    course = courseName
                });

                return PaymentResult.Successful("Payment processed successfully", gatewayResponse);
           
        }
    }
}
