
namespace SmartLearning.Application.Services
{
    public class UnitService : IUnitService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UnitService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UnitResponseDto> AddUnitAsync(CreateUnitDto dto)
        {
            var unit = _mapper.Map<Unit>(dto);

            await _unitOfWork.Repository<Unit>().AddAsync(unit);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<UnitResponseDto>(unit);
        }

        public async Task<UnitResponseDto?> GetUnitByIdAsync(int id)
        {
            var result = await _unitOfWork.Repository<Unit>().FindAsync(
        u => u.Unit_Id == id,
        u => u.Course,
        u => u.Course.Instructor);

            var unit = result.FirstOrDefault();
            return unit == null ? null : _mapper.Map<UnitResponseDto>(unit);
        }

        public async Task<IReadOnlyList<UnitResponseDto>> GetUnitsByCourseIdAsync(int courseId)
        {
            var units = await _unitOfWork.Repository<Unit>().FindAsync(
          u => u.Crs_Id == courseId,
          u => u.Course,
          u => u.Course.Instructor
      );

            return _mapper.Map<IReadOnlyList<UnitResponseDto>>(units);
        }

        public async Task UpdateUnitAsync(int id, UpdateUnitDto dto)
        {
            var unit = await _unitOfWork.Repository<Unit>().GetByIdAsync(id);
            if (unit == null) throw new KeyNotFoundException("Unit not found");

            _mapper.Map(dto, unit);

            _unitOfWork.Repository<Unit>().Update(unit);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteUnitAsync(int id)
        {
            var unit = await _unitOfWork.Repository<Unit>().GetByIdAsync(id);
            if (unit != null)
            {
                _unitOfWork.Repository<Unit>().Remove(unit);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}

