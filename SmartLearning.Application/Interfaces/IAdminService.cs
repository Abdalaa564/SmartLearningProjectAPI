
namespace SmartLearning.Application.Interfaces
{
    public interface IAdminService
    {
        Task<IEnumerable<AdminResponseDto>> GetAllAsync();
        Task<AdminResponseDto?> GetByIdAsync(string userId);
        Task<AdminResponseDto> CreateAsync(CreateAdminDto dto);
        Task<bool> UpdateAsync(string userId, UpdateAdminDto dto);
        Task<bool> DeleteAsync(string userId);
        Task<int> GetAdminsCountAsync();
    }
}
