


namespace SmartLearning.Application.Services
{
    public class InstructorService : IInstructorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public InstructorService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET ALL
        public async Task<IEnumerable<InstructorResponseDto>> GetAllAsync()
        {
            var repo = _unitOfWork.Repository<Instructor>();

            var instructors = await repo.GetAllAsync(query =>
                query
                    .Include(i => i.Courses)
                        .ThenInclude(c => c.Enrollments)
            );

            return _mapper.Map<IEnumerable<InstructorResponseDto>>(instructors);
        }

        // GET BY ID
        public async Task<InstructorResponseDto?> GetByIdAsync(int id)
        {
            var repo = _unitOfWork.Repository<Instructor>();

            var instructors = await repo.FindAsync(
                i => i.Id == id,
                q => q
                    .Include(i => i.Courses)
                        .ThenInclude(c => c.Enrollments)
            );

            var instructor = instructors.FirstOrDefault();
            if (instructor == null)
                return null;

            return _mapper.Map<InstructorResponseDto>(instructor);
        }

        // CREATE
        public async Task<InstructorResponseDto> CreateAsync(CreateInstructorDto dto)
        {
            var repo = _unitOfWork.Repository<Instructor>();

            var instructor = _mapper.Map<Instructor>(dto);

            await repo.AddAsync(instructor);
            await _unitOfWork.CompleteAsync();

            // نرجّع المدرّس مع الـ User + Courses + Enrollments
            var loadedList = await repo.FindAsync(
                i => i.Id == instructor.Id,
                q => q.Include(i => i.User)
                      .Include(i => i.Courses)
                          .ThenInclude(c => c.Enrollments)
            );

            var loaded = loadedList.FirstOrDefault() ?? instructor;

            return _mapper.Map<InstructorResponseDto>(loaded);
        }

        // UPDATE
        public async Task<bool> UpdateAsync(int id, UpdateInstructorDto dto)
        {
            var repo = _unitOfWork.Repository<Instructor>();

            var instructors = await repo.FindAsync(
                i => i.Id == id,
                q => q.Include(i => i.User)
            );

            var instructor = instructors.FirstOrDefault();
            if (instructor == null)
                return false;

            _mapper.Map(dto, instructor);

            repo.Update(instructor);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var repo = _unitOfWork.Repository<Instructor>();

            var instructors = await repo.FindAsync(i => i.Id == id);
            var instructor = instructors.FirstOrDefault();

            if (instructor == null)
                return false;

            repo.Remove(instructor);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}