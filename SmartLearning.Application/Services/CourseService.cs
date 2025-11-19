




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

        public async Task<IEnumerable<CourseResponseDTO>> GetAllCourseAsync()
        {
            var courses = await _unitOfWork.Repository<Course>()
                .GetAllAsync(c => c.User,c => c.Units);

            return _mapper.Map<IEnumerable<CourseResponseDTO>>(courses);
        }
    }
}


//.GetAllAsync(q => q
//    .Include(c => c.User)
//    .Include(c => c.Units)
//    .ThenInclude(u => u.Lessons)
//)