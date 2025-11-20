






namespace SmartLearning.Application.Services
{
    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CourseService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CourseResponseDto>> GetAllCourseAsync()
        {
            var result = await _unitOfWork.Repository<Course>()
                .GetAllAsync(c => c.User);

            return _mapper.Map<IEnumerable<CourseResponseDto>>(result);
        }

        public async Task<CourseResponseDto?> GetByIdAsync(int id)
        {
            var course = await _unitOfWork.Repository<Course>()
        .FindAsync(c => c.Crs_Id == id, c => c.User);

            var entity = course.FirstOrDefault();
            return _mapper.Map<CourseResponseDto>(entity);
        }

        public async Task<bool> AddCourseAsync(AddCourseDto dto)
        {
            var course = _mapper.Map<Course>(dto);

            await _unitOfWork.Repository<Course>().AddAsync(course);
            return await _unitOfWork.CompleteAsync() > 0;
        }

        public async Task<bool> UpdateCourseAsync(int id, UpdateCourseDto dto)
        {
            var course = await _unitOfWork.Repository<Course>().GetByIdAsync(id);
            if (course == null) return false;

            _mapper.Map(dto, course);

            _unitOfWork.Repository<Course>().Update(course);
            return await _unitOfWork.CompleteAsync() > 0;
        }

        public async Task<bool> DeleteCourseAsync(int id)
        {
            var course = await _unitOfWork.Repository<Course>().GetByIdAsync(id);
            if (course == null) return false;

            _unitOfWork.Repository<Course>().Remove(course);
            return await _unitOfWork.CompleteAsync() > 0;
        }
    }
}


//.GetAllAsync(q => q
//    .Include(c => c.User)
//    .Include(c => c.Units)
//    .ThenInclude(u => u.Lessons)
//)