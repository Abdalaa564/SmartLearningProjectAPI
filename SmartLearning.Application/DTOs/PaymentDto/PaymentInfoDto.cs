namespace SmartLearning.Application.DTOs.PaymentDto
{
    public class PaymentInfoDto
    {
        [Required]
        [MaxLength(50)]
        public string PaymentMethod { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }

        [MaxLength(100)]
        public string? CardHolderName { get; set; }

        [MaxLength(4)]
        public string? CardLastFourDigits { get; set; }
    }
}
