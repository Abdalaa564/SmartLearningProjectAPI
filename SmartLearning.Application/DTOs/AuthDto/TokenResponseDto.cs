namespace SmartLearning.Application.DTOs.AuthDto
{
    public class TokenResponseDto
    {
        public string AccessToken { get; set; }
       
        public int ExpiresIn { get; set; }
        public string TokenType { get; set; } = "Bearer";
    }
}
