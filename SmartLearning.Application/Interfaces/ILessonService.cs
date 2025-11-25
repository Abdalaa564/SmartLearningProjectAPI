
namespace SmartLearning.Application.Interfaces
{
    public interface ILessonService
    {
        Task<LessonResponseDto> AddLessonAsync(CreateLessonDto dto);
        Task<LessonResponseDto?> GetLessonByIdAsync(int id);
        Task<IReadOnlyList<LessonResponseDto>> GetLessonsByUnitIdAsync(int unitId);
        Task UpdateLessonAsync(int id, UpdateLessonDto dto);
        Task DeleteLessonAsync(int id);
        Task<LessonDetailsDto?> GetLessonWithResourcesByIdAsync(int id);
    }
}
