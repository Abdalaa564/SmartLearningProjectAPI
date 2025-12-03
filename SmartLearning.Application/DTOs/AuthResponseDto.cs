
namespace SmartLearning.Application.DTOs
{
    public class AuthResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object? Data { get; set; }
        //public StudentProfileDto? Data { get; set; }
        //public InstructorResponseDto? Instructor { get; set; }
        public TokenResponseDto Token { get; set; }
    }
}
