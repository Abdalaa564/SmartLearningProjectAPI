

namespace SmartLearning.Application.DTOs.PaymentDto
{
    public class PaymentResult
    {
        public bool Success { get; private set; }
        public string Message { get; private set; } = string.Empty;
        public string? GatewayResponse { get; private set; }

        private PaymentResult(bool success, string message, string? gatewayResponse)
        {
            Success = success;
            Message = message;
            GatewayResponse = gatewayResponse;
        }

        public static PaymentResult Successful(string message, string? gatewayResponse)
            => new PaymentResult(true, message, gatewayResponse);

        public static PaymentResult Failed(string message)
            => new PaymentResult(false, message, null);
    }

}
