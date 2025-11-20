using Microsoft.AspNetCore.Identity;

namespace SmartLearning.Application.Services
{
    public class InstructorService: IInstructorService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public InstructorService(UserManager<ApplicationUser> userManager, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<CreateInstructorDto>> GetAllInstructorAsync()
        {
            var instructors = await _userManager.GetUsersInRoleAsync("Instructor");
            return _mapper.Map<IEnumerable<CreateInstructorDto>>(instructors);
        }

        public async Task<CreateInstructorDto?> GetByIdAsync(string id)
        {
            var repo = _unitOfWork.Repository<ApplicationUser>();

            // جلب المستخدم حسب CustomNumberId
            var user = (await repo.FindAsync(u => u.Id==id)).FirstOrDefault();

            if (user == null) return null;

            return _mapper.Map<CreateInstructorDto>(user);
        }
        public async Task<CreateInstructorDto> AddInstructorAsync(CreateInstructorDto dto)
        {
            var instructor = _mapper.Map<ApplicationUser>(dto);
            await _unitOfWork.Repository<ApplicationUser>().AddAsync(instructor);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<CreateInstructorDto>(instructor);
        }

        public async Task<bool> UpdateAsync(string id, UpdateInstructorDto dto)
        {
            var repo = _unitOfWork.Repository<ApplicationUser>();
            var user = (await repo.FindAsync(u => u.Id==id)).FirstOrDefault();

            if (user == null) return false;

            _mapper.Map(dto, user);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var repo = _unitOfWork.Repository<ApplicationUser>();

            var user = (await repo.FindAsync(u => u.Id==id)).FirstOrDefault();
            if (user == null) return false;

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

    }
}
