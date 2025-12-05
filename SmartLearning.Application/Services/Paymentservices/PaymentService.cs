using SmartLearning.Application.DTOs.PaymentDto;
using System.Net.Http.Json;
using System.Text.Json;

namespace SmartLearning.Application.Services.Paymentservices
{
    public class PaymentService : IPaymentService
    {
        private readonly HttpClient _httpClient;
        private readonly PaymobSettings _paymobSettings;
        public PaymentService(HttpClient httpClient, PaymobSettings paymobSettings)
        {
            _httpClient = httpClient;
            _paymobSettings = paymobSettings;
        }
        //Step 1: Authenticate with Paymob to get auth token
        private async Task<string> GetAuthTokenAsync()
        {
            Console.WriteLine($"{_paymobSettings.ApiUrl}/auth/tokens");

            var authRequest = new
            {
                api_key = _paymobSettings.ApiKey
            };

            var response = await _httpClient.PostAsJsonAsync(
                $"{_paymobSettings.ApiUrl}/auth/tokens",
                authRequest
            );

            response.EnsureSuccessStatusCode();
            var authResponse = await response.Content.ReadFromJsonAsync<PaymobAuthResponse>();
            return authResponse.Token;
        }
        //Step 2: Create order in Paymob
        private async Task<int> CreateOrderAsync(string authToken, decimal amount, string transactionId)
        {
            var orderRequest = new
            {
                auth_token = authToken,
                delivery_needed = "false",
                amount_cents = (int)(amount * 100), // Convert to cents
                currency = "EGP",
                merchant_order_id = transactionId,
                items = new[] 
                { 
                    new
                    {
                        name = "Course Purchase",
                        amount_cents = (int)(amount * 100),
                        quantity = 1
                     }

                }
            };

            var response = await _httpClient.PostAsJsonAsync(
                $"{_paymobSettings.ApiUrl}/ecommerce/orders",
                orderRequest
            );

            response.EnsureSuccessStatusCode();
            var orderResponse = await response.Content.ReadFromJsonAsync<PaymobOrderResponse>();
            return orderResponse.Id;
        }

        //Step 3: Generate payment key for iframe/redirect
        private async Task<string> GetPaymentKeyAsync(
            string authToken,
            int orderId,
            decimal amount,
            PaymentInfoDto payment,
            string studentEmail,
            string studentPhone,
            string studentName)
        {
            var nameParts = studentName.Split(' ', 2);
            var firstName = nameParts.Length > 0 ? nameParts[0] : "Student";
            var lastName = nameParts.Length > 1 ? nameParts[1] : "User";

            var paymentKeyRequest = new
            {
                auth_token = authToken,
                amount_cents = (int)(amount * 100),
                expiration = 3600, // 1 hour
                order_id = orderId,
                billing_data = new
                {
                    apartment = "NA",
                    email = studentEmail,
                    floor = "NA",
                    first_name = firstName,
                    street = "NA",
                    building = "NA",
                    phone_number = studentPhone,
                    shipping_method = "NA",
                    postal_code = "NA",
                    city = "NA",
                    country = "EG",
                    last_name = lastName,
                    state = "NA"
                },
                currency = "EGP",
                integration_id = _paymobSettings.IntegrationId
            };

            var response = await _httpClient.PostAsJsonAsync(
                $"{_paymobSettings.ApiUrl}/acceptance/payment_keys",
                paymentKeyRequest
            );

            response.EnsureSuccessStatusCode();
            var keyResponse = await response.Content.ReadFromJsonAsync<PaymobPaymentKeyResponse>();
            return keyResponse.Token;
        }




        public async Task<string> InitiatePaymobPaymentAsync(PaymentInfoDto payment, string transactionId, string courseName, string studentEmail, string studentPhone, string studentName)
        {
            // Step 1: Get authentication token
            var authToken = await GetAuthTokenAsync();

            // Step 2: Create order
            var orderId = await CreateOrderAsync(authToken, payment.Amount, transactionId);

            // Step 3: Get payment key
            var paymentToken = await GetPaymentKeyAsync(
                authToken,
                orderId,
                payment.Amount,
                payment,
                studentEmail,
                studentPhone,
                studentName
            );

            // Return iframe URL for frontend
            return $"https://accept.paymob.com/api/acceptance/iframes/{_paymobSettings.IframeId}?payment_token={paymentToken}";
        }

       

        public async Task<PaymentResult> VerifyPaymobCallbackAsync(Dictionary<string, string> callbackData)
        {
            // Validate HMAC signature
            if (!ValidateHmacSignature(callbackData))
            {
                return PaymentResult.Failed("Invalid payment signature");
            }

            var success = callbackData.ContainsKey("success") && callbackData["success"] == "true";
            var transactionId = callbackData.GetValueOrDefault("merchant_order_id");
            var amount = decimal.Parse(callbackData.GetValueOrDefault("amount_cents", "0")) / 100;
            var paymentMethod = callbackData.GetValueOrDefault("source_data.type");

            if (success)
            {
                var gatewayResponse = JsonSerializer.Serialize(new
                {
                    gateway = "Paymob",
                    status = "approved",
                    transaction_id = transactionId,
                    paymob_transaction_id = callbackData.GetValueOrDefault("id"),
                    timestamp = DateTime.UtcNow,
                    payment_method = paymentMethod,
                    amount = amount,
                    raw_callback = callbackData
                });

                return PaymentResult.Successful("Payment processed successfully via Paymob", gatewayResponse);
            }
            else
            {
                return PaymentResult.Failed($"Payment failed: {callbackData.GetValueOrDefault("pending", "Unknown error")}");
            }
        }
        private bool ValidateHmacSignature(Dictionary<string, string> data)
        {
            if (!data.ContainsKey("hmac"))
                return false;

            var receivedHmac = data["hmac"];

            // Build concatenated string according to Paymob docs
            var concatenatedString = string.Join("", new[]
            {
                data.GetValueOrDefault("amount_cents"),
                data.GetValueOrDefault("created_at"),
                data.GetValueOrDefault("currency"),
                data.GetValueOrDefault("error_occured"),
                data.GetValueOrDefault("has_parent_transaction"),
                data.GetValueOrDefault("id"),
                data.GetValueOrDefault("integration_id"),
                data.GetValueOrDefault("is_3d_secure"),
                data.GetValueOrDefault("is_auth"),
                data.GetValueOrDefault("is_capture"),
                data.GetValueOrDefault("is_refunded"),
                data.GetValueOrDefault("is_standalone_payment"),
                data.GetValueOrDefault("is_voided"),
                data.GetValueOrDefault("order"),
                data.GetValueOrDefault("owner"),
                data.GetValueOrDefault("pending"),
                data.GetValueOrDefault("source_data.pan"),
                data.GetValueOrDefault("source_data.sub_type"),
                data.GetValueOrDefault("source_data.type"),
                data.GetValueOrDefault("success")
            });

            // Calculate HMAC
            using var hmac = new System.Security.Cryptography.HMACSHA512(
               Encoding.UTF8.GetBytes(_paymobSettings.HmacSecret)
            );
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(concatenatedString));
            var calculatedHmac = BitConverter.ToString(hash).Replace("-", "").ToLower();

            return calculatedHmac == receivedHmac.ToLower();
        }

        public async Task<PaymentResult> ProcessPaymentAsync(
           PaymentInfoDto payment,
           string transactionId,
           string courseName)
        {

            // For Paymob integration, this method should be called AFTER
            // successful callback verification
            await Task.Delay(100);

            var gatewayResponse = JsonSerializer.Serialize(new
            {
                gateway = "Paymob",
                status = "pending_verification",
                transaction_id = transactionId,
                timestamp = DateTime.UtcNow,
                payment_method = payment.PaymentMethod,
                amount = payment.Amount,
                course = courseName,
                note = "Awaiting Paymob callback verification"
            });

            return PaymentResult.Successful("Payment initiated, awaiting confirmation", gatewayResponse);

        }

    }
}
