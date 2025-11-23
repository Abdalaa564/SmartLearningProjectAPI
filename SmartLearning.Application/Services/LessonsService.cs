
namespace SmartLearning.Application.Services
{
    public class LessonsService : ILessonService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LessonsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<LessonResponseDto> AddLessonAsync(CreateLessonDto dto)
        {
            var lesson = _mapper.Map<Lessons>(dto);

            await _unitOfWork.Repository<Lessons>().AddAsync(lesson);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<LessonResponseDto>(lesson);
        }

        public async Task<LessonResponseDto?> GetLessonByIdAsync(int id)
        {
            var lesson = await _unitOfWork.Repository<Lessons>().GetByIdAsync(id);
            return lesson == null ? null : _mapper.Map<LessonResponseDto>(lesson);
        }

        public async Task<IReadOnlyList<LessonResponseDto>> GetLessonsByUnitIdAsync(int unitId)
        {
            var lessons = await _unitOfWork.Repository<Lessons>().FindAsync(l => l.Unit_Id == unitId);
            return _mapper.Map<IReadOnlyList<LessonResponseDto>>(lessons);
        }

        public async Task UpdateLessonAsync(int id, UpdateLessonDto dto)
        {
            var lesson = await _unitOfWork.Repository<Lessons>().GetByIdAsync(id);
            if (lesson == null) throw new KeyNotFoundException("Lesson not found");

            _mapper.Map(dto, lesson);

            _unitOfWork.Repository<Lessons>().Update(lesson);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteLessonAsync(int id)
        {
            var lesson = await _unitOfWork.Repository<Lessons>().GetByIdAsync(id);
            if (lesson != null)
            {
                _unitOfWork.Repository<Lessons>().Remove(lesson);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}