

namespace SmartLearning.Application.Interfaces
{
    public interface IInstructorService
    {
        Task<IEnumerable<CreateInstructorDto>> GetAllInstructorAsync();
        Task<CreateInstructorDto?> GetByIdAsync(int id);
        Task<CreateInstructorDto> AddInstructorAsync(CreateInstructorDto dto);
        Task<bool> UpdateAsync(int id, UpdateInstructorDto dto);
        Task<bool> DeleteAsync(int CustomNumberId);
    }
}
