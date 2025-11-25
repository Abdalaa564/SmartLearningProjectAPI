

using SmartLearning.Application.DTOs.InstructorDto;
using SmartLearning.Application.DTOs.Instructors;

namespace SmartLearning.Application.Interfaces
{
    public interface IInstructorService
    {
        Task<IEnumerable<InstructorResponseDto>> GetAllAsync();
        Task<InstructorResponseDto?> GetByIdAsync(int id);
        Task<InstructorResponseDto> CreateAsync(CreateInstructorDto dto);
        Task<bool> UpdateAsync(int id, UpdateInstructorDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
