


namespace SmartLearning.Application.Services
{
    public class InstructorService : IInstructorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public InstructorService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        // GET ALL
        public async Task<IEnumerable<InstructorResponseDto>> GetAllAsync()
        {
            var repo = _unitOfWork.Repository<Instructor>();

            var instructors = await repo.GetAllAsync(query =>
                query
                     .Include(i => i.User)
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
                    .Include(i => i.User)
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
            // 1) Check if Email already exists
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
                throw new Exception("Email already used by another account");

            // 2) Create ApplicationUser
            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                throw new Exception(string.Join("; ", result.Errors.Select(e => e.Description)));

            // 3) Assign Role Instructor
            await _userManager.AddToRoleAsync(user, "Instructor");

            // 4) Create Instructor row
            var repo = _unitOfWork.Repository<Instructor>();
            var instructor = new Instructor
            {
                UserId = user.Id,
                FullName = dto.FullName,
                JobTitle = dto.JobTitle,
                PhoneNumber = dto.PhoneNumber,
                PhotoUrl = dto.PhotoUrl,
                YoutubeChannelUrl = dto.YoutubeChannelUrl,
                CertificateUrl = dto.CertificateUrl,
                Rating = dto.Rating
            };

            await repo.AddAsync(instructor);
            await _unitOfWork.CompleteAsync();

            // 5) Load and return
            var loadedData = await repo.FindAsync(i => i.Id == instructor.Id, q => q.Include(i => i.User));

            return _mapper.Map<InstructorResponseDto>(loadedData.First());
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


        // to get profile 

        public async Task<InstructorResponseDto?> GetByUserIdProfilrAsync(string userId)
        {
            var repo = _unitOfWork.Repository<Instructor>();

            var instructors = await repo.FindAsync(
                i => i.UserId == userId,
                q => q
                    .Include(i => i.Courses)
                        .ThenInclude(c => c.Enrollments)
            );

            var instructor = instructors.FirstOrDefault();
            return instructor == null ? null : _mapper.Map<InstructorResponseDto>(instructor);
        }

        //get count of instructors
        public async Task<int> GetInstructorsCountAsync()
        {
            var repo = _unitOfWork.Repository<Instructor>();

            var instructors = await repo.GetAllAsync();
            return instructors.Count;
        }

        // Register Instructor
        public async Task<InstructorResponseDto> RegisterInstructorAsync(RegisterInstructorDto dto)
        {
            // 1) check email exists
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
                throw new Exception("Email already used by another account");

            // 2) create ApplicationUser
            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber
            };

            var createUserResult = await _userManager.CreateAsync(user, dto.Password);
            if (!createUserResult.Succeeded)
                throw new Exception(string.Join("; ", createUserResult.Errors.Select(e => e.Description)));

            // 3) add role "Instructor"
            await _userManager.AddToRoleAsync(user, "Instructor");

            // 4) create Instructor as Pending
            var repo = _unitOfWork.Repository<Instructor>();
            var instructor = new Instructor
            {
                UserId = user.Id,
                FullName = dto.FullName,
                JobTitle = dto.JobTitle,
                PhoneNumber = dto.PhoneNumber,
                YoutubeChannelUrl = dto.YoutubeChannelUrl,
                PhotoUrl = dto.PhotoUrl,
                CertificateUrl = dto.CertificateUrl,

                CvUrl = dto.CvUrl,
                Specialization = dto.Specialization,
                UniversityName = dto.UniversityName,
                About = dto.About,

                Rating = null,
                Status = InstructorStatus.Pending
            };

            await repo.AddAsync(instructor);
            await _unitOfWork.CompleteAsync();

            var loaded = await repo.FindAsync(i => i.Id == instructor.Id,
                q => q.Include(i => i.User));

            return _mapper.Map<InstructorResponseDto>(loaded.First());
        }
        // Get all pending instructors
        public async Task<IEnumerable<InstructorResponseDto>> GetPendingInstructorsAsync()
        {
            var repo = _unitOfWork.Repository<Instructor>();
            var instructors = await repo.FindAsync(
                i => i.Status == InstructorStatus.Pending,
                q => q.Include(i => i.User)
            );

            return _mapper.Map<IEnumerable<InstructorResponseDto>>(instructors);
        }
        // Approve instructor
        public async Task<bool> ApproveInstructorAsync(int id)
        {
            var repo = _unitOfWork.Repository<Instructor>();
            var instructors = await repo.FindAsync(i => i.Id == id, q => q.Include(i => i.User));
            var instructor = instructors.FirstOrDefault();
            if (instructor == null) return false;

            instructor.Status = InstructorStatus.Approved;

            repo.Update(instructor);
            await _unitOfWork.CompleteAsync();
            return true;
        }
        // reject instructor
        public async Task<bool> RejectInstructorAsync(int id)
        {
            var repo = _unitOfWork.Repository<Instructor>();
            var instructors = await repo.FindAsync(i => i.Id == id);
            var instructor = instructors.FirstOrDefault();
            if (instructor == null) return false;

            instructor.Status = InstructorStatus.Rejected;

            repo.Update(instructor);
            await _unitOfWork.CompleteAsync();
            return true;
        }

    }
}