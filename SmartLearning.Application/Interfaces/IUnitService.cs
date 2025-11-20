
namespace SmartLearning.Application.Interfaces
{
    public interface IUnitService
    {
        Task<UnitResponseDto> AddUnitAsync(CreateUnitDto dto);
        Task<UnitResponseDto?> GetUnitByIdAsync(int id);
        Task<IReadOnlyList<UnitResponseDto>> GetUnitsByCourseIdAsync(int courseId);
        Task UpdateUnitAsync(int id, UpdateUnitDto dto);
        Task DeleteUnitAsync(int id);
    }
}
