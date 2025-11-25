


namespace SmartLearning.Application.Interfaces
{
    public interface IInstructorService
    {
        Task<IEnumerable<InstructorResponseDto>> GetAllAsync();
        Task<InstructorResponseDto?> GetByIdAsync(int id);
        Task<InstructorResponseDto> CreateAsync(DTOs.CreateInstructorDto dto);
        Task<bool> UpdateAsync(int id, DTOs.UpdateInstructorDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
