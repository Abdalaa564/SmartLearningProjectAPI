
using SmartLearning.Application.DTOs.Resource;

namespace SmartLearning.Application.Services
{
    public class ResourceService : IResourceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ResourceService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResourceResponseDto> AddResourceAsync(CreateResourceDto dto)
        {
            var resource = _mapper.Map<Resource>(dto);

            await _unitOfWork.Repository<Resource>().AddAsync(resource);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<ResourceResponseDto>(resource);
        }

        public async Task<ResourceResponseDto?> GetByIdAsync(int id)
        {
            var resource = await _unitOfWork.Repository<Resource>().GetByIdAsync(id);
            return resource == null ? null : _mapper.Map<ResourceResponseDto>(resource);
        }

        public async Task<IReadOnlyList<ResourceResponseDto>> GetByLessonIdAsync(int lessonId)
        {
            var resources = await _unitOfWork.Repository<Resource>()
                                              .FindAsync(r => r.Lesson_Id == lessonId);

            return _mapper.Map<IReadOnlyList<ResourceResponseDto>>(resources);
        }

        public async Task DeleteAsync(int id)
        {
            var resource = await _unitOfWork.Repository<Resource>().GetByIdAsync(id);
            if (resource != null)
            {
                _unitOfWork.Repository<Resource>().Remove(resource);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}