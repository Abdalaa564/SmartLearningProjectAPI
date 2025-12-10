

namespace SmartLearning.Application.Services
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IEnumerable<AdminResponseDto>> GetAllAsync()
        {
            var admins = await _userManager.GetUsersInRoleAsync("Admin");
            return admins.Select(u => new AdminResponseDto
            {
                Id = u.Id,
                UserName = u.UserName ?? "",
                Email = u.Email ?? ""
            });
        }

        public async Task<AdminResponseDto?> GetByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return null;

            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            if (!isAdmin) return null;

            return new AdminResponseDto
            {
                Id = user.Id,
                UserName = user.UserName ?? "",
                Email = user.Email ?? ""
            };
        }

        public async Task<AdminResponseDto> CreateAsync(CreateAdminDto dto)
        {
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
                throw new Exception("Email already exists");

            var user = new ApplicationUser
            {
                UserName = dto.UserName,
                Email = dto.Email
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                throw new Exception(string.Join("; ", result.Errors.Select(e => e.Description)));

            await _userManager.AddToRoleAsync(user, "Admin");

            return new AdminResponseDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email
            };
        }

        public async Task<bool> UpdateAsync(string userId, UpdateAdminDto dto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            if (!isAdmin) return false;

            if (!string.IsNullOrEmpty(dto.UserName))
                user.UserName = dto.UserName;

            if (!string.IsNullOrEmpty(dto.Email))
                user.Email = dto.Email;

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> DeleteAsync(string userId)
        {
            var adminsCount = await GetAdminsCountAsync();
            if (adminsCount <= 1)
                throw new Exception("Cannot delete the last admin");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            if (!isAdmin) return false;

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<int> GetAdminsCountAsync()
        {
            var admins = await _userManager.GetUsersInRoleAsync("Admin");
            return admins.Count;
        }
    }
}
