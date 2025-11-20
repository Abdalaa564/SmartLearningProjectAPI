using SmartLearning.Application.DTOs.LessonDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLearning.Application.Interfaces
{
    public interface ILessonService
    {
        Task<LessonResponseDto> AddLessonAsync(CreateLessonDto dto);
        Task<LessonResponseDto?> GetLessonByIdAsync(int id);
        Task<IReadOnlyList<LessonResponseDto>> GetLessonsByUnitIdAsync(int unitId);
        Task UpdateLessonAsync(int id, UpdateLessonDto dto);
        Task DeleteLessonAsync(int id);
    }
}
