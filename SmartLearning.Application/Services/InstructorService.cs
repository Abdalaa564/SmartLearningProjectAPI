using Microsoft.AspNetCore.Identity;
using SmartLearning.Application.GenericInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var user = await _userManager.FindByIdAsync(id);
            if (user == null || !await _userManager.IsInRoleAsync(user, "Instructor"))
                return null;

            return _mapper.Map<CreateInstructorDto>(user);
        }
        public async Task<CreateInstructorDto> AddInstructorAsync(UpdateInstructorDto dto)
        {
            var instructor = _mapper.Map<ApplicationUser>(dto);
            await _unitOfWork.Repository<ApplicationUser>().AddAsync(instructor);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<CreateInstructorDto>(instructor);
        }

        public async Task<bool> UpdateAsync(string id, UpdateInstructorDto dto)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return false;

            _mapper.Map(dto, user);
            await _userManager.UpdateAsync(user);

            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return false;

            await _userManager.DeleteAsync(user);
            return true;
        }
    }
}
