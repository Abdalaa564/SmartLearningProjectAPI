
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

            // Include الـ User عشان لو بتحتاج Email أو بيانات تانية في الـ Response
            var instructors = await repo.GetAllAsync(i => i.User);

            return _mapper.Map<IEnumerable<InstructorResponseDto>>(instructors);
        }

        // GET BY ID
        public async Task<InstructorResponseDto?> GetByIdAsync(int id)
        {
            var repo = _unitOfWork.Repository<Instructor>();

            // جلب المستخدم حسب CustomNumberId
            var user = (await repo.FindAsync(u => u.Id==id)).FirstOrDefault();
            var instructors = await repo.FindAsync(
                i => i.Id == id,
                i => i.User
            );

            var instructor = instructors.FirstOrDefault();
            if (instructor == null)
                return null;

            return _mapper.Map<InstructorResponseDto>(instructor);
        }

        // CREATE
        public async Task<InstructorResponseDto> CreateAsync(DTOs.CreateInstructorDto dto)
        {
            var repo = _unitOfWork.Repository<Instructor>();

            // Map من DTO لـ Entity
            var instructor = _mapper.Map<Instructor>(dto);

            await repo.AddAsync(instructor);
            await _unitOfWork.CompleteAsync();

            // نعمل reload للإنستراكتور مع include User (لو محتاج Email في الـ Response)
            var loaded = (await repo.FindAsync(i => i.Id == instructor.Id, i => i.User))
                         .FirstOrDefault() ?? instructor;

            return _mapper.Map<InstructorResponseDto>(loaded);
        }

        // UPDATE
        public async Task<bool> UpdateAsync(int id, DTOs.UpdateInstructorDto dto)
        {
            var repo = _unitOfWork.Repository<Instructor>();

            var instructors = await repo.FindAsync(
                i => i.Id == id,
                i => i.User
            );

            var instructor = instructors.FirstOrDefault();
            if (instructor == null)
                return false;

            // AutoMapper هيحدّث بس الفيلدز اللي مش null (لو مجهّز الـ Profile صح)
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