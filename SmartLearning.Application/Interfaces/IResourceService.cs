

using SmartLearning.Application.DTOs.Resource;

namespace SmartLearning.Application.Interfaces
{
    public interface IResourceService
    {
        Task<ResourceResponseDto> AddResourceAsync(CreateResourceDto dto);
        Task<ResourceResponseDto?> GetByIdAsync(int id);
        Task<IReadOnlyList<ResourceResponseDto>> GetByLessonIdAsync(int lessonId);
        Task DeleteAsync(int id);
    }
}
