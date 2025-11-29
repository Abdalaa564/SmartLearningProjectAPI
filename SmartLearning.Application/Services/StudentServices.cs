
namespace SmartLearning.Application.Services
{
    public class StudentServices : IStudentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;



        public StudentServices(IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _tokenService = tokenService;
        }
        public async Task<StudentProfileDto> GetStudentProfileAsync(string userId)
        {
            var user = await _unitOfWork.Repository<Student>()
                .FindAsync(s => s.UserId == userId, s => s.User);

            var foundUser = user.FirstOrDefault();
            if (foundUser == null)
                throw new Exception("Student profile not found");

           
            var profileDto = _mapper.Map<StudentProfileDto>(foundUser);
            return profileDto;
        }

        public async Task<AuthResponseDto> RegisterStudentAsync(RegisterStudentDto registerDto)
        {
            var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
            if (existingUser != null)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Email already registered"
                };
            }

            
           // var applicationUser = _mapper.Map<ApplicationUser>(registerDto);

            var applicationUser = new ApplicationUser
            {
                UserName = registerDto.Email,
                Email = registerDto.Email
            };

            // Create user with Identity
            var result = await _userManager.CreateAsync(applicationUser, registerDto.Password);

            if (!result.Succeeded)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = string.Join(", ", result.Errors.Select(e => e.Description))
                };
            }


            // await _userManager.AddToRoleAsync(applicationUser, "Student");

            var student = new Student
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                PhoneNumber = registerDto.PhoneNumber,
                DateOfBirth = registerDto.DateOfBirth,
                Address = registerDto.Address,
                City = registerDto.City,
                CreatedDate = DateTime.UtcNow,
               // EnrollmentNumber = GenerateEnrollmentNumber(),
                UserId = applicationUser.Id
            };

            await _unitOfWork.Repository<Student>().AddAsync(student);
            await _unitOfWork.CompleteAsync();

           
            var profileDto = _mapper.Map<StudentProfileDto>(student);
            var token = await _tokenService.GenerateTokenAsync(applicationUser);

            return new AuthResponseDto
            {
                Success = true,
                Message = "Registration successful",
                Data = profileDto,
                Token = token
            };
        }

        public async Task<StudentProfileDto> UpdateStudentAsync(string userId, StudentUpdateDto updateDto)
        {
            var students = await _unitOfWork.Repository<Student>()
                     .FindAsync(s => s.UserId == userId, s => s.User);

            var student = students.FirstOrDefault();
            if (student == null)
                throw new Exception("Student profile not found");

            // Map updates to student
            _mapper.Map(updateDto, student);
            student.UpdatedDate = DateTime.UtcNow;

            _unitOfWork.Repository<Student>().Update(student);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<StudentProfileDto>(student);
        }
    }
}
