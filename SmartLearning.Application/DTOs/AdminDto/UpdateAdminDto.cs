
namespace SmartLearning.Application.DTOs.AdminDto
{
    public class UpdateAdminDto
    {
        [MaxLength(100)]
        public string? UserName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }
    }
}
