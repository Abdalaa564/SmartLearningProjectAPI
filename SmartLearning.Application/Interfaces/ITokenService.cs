
using SmartLearning.Application.DTOs.AuthDto;

namespace SmartLearning.Application.Interfaces
{
    public interface ITokenService
    {
        Task<TokenResponseDto> GenerateTokenAsync(ApplicationUser user);
       
    }
}
