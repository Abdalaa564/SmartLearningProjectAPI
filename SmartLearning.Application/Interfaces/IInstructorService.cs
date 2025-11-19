

namespace SmartLearning.Application.Interfaces
{
    public interface IInstructorService
    {
        Task<IEnumerable<CreateInstructorDto>> GetAllInstructorAsync();
        Task<CreateInstructorDto?> GetByIdAsync(string id);
        Task<CreateInstructorDto> AddInstructorAsync(UpdateInstructorDto dto);
        Task<bool> UpdateAsync(string id, UpdateInstructorDto dto);
        Task<bool> DeleteAsync(string id);
    }
}
