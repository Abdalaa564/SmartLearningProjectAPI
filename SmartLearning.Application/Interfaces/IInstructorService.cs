


namespace SmartLearning.Application.Interfaces
{
    public interface IInstructorService
    {
        Task<IEnumerable<InstructorResponseDto>> GetAllAsync();
        Task<InstructorResponseDto?> GetByIdAsync(int id);
        Task<InstructorResponseDto> CreateAsync(CreateInstructorDto dto);
        Task<bool> UpdateAsync(int id, UpdateInstructorDto dto);
        Task<bool> DeleteAsync(int id);

        Task<InstructorResponseDto?> GetByUserIdProfilrAsync(string userId);

        Task<int> GetInstructorsCountAsync();

        // new 
        Task<InstructorResponseDto> RegisterInstructorAsync(RegisterInstructorDto dto);
        Task<IEnumerable<InstructorResponseDto>> GetPendingInstructorsAsync();
        Task<bool> ApproveInstructorAsync(int id);
        Task<bool> RejectInstructorAsync(int id);

    }
}
