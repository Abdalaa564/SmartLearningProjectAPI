
namespace SmartLearningProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration config;
        private readonly IStudentService _studentService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IInstructorService _instructorService;

        public AccountController(
            IInstructorService instructorService,
            UserManager<ApplicationUser> userManager, 
            IConfiguration config, IStudentService studentService,
            SignInManager<ApplicationUser> signInManager, ITokenService tokenService)
        {
            this.userManager = userManager;
            this.config = config;
            _studentService = studentService;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _instructorService = instructorService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register( RegisterStudentDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _studentService.RegisterStudentAsync(registerDto);

            if (!result.Success)
                return BadRequest(result);

            return CreatedAtAction(nameof(GetMyProfile),  result);
        }

        [HttpPost("login")]
        
        public async Task<ActionResult<AuthResponseDto>> Login( LoginUserDto loginDto)
        {
            var user = await userManager.FindByNameAsync(loginDto.Email);
            if (user == null)
                return Unauthorized(new { message = "Invalid credentials" });

            var result = await _signInManager.PasswordSignInAsync(user.UserName, loginDto.Password, false, false);

            if (!result.Succeeded)
                return Unauthorized(new { message = "Invalid credentials" });

            var roles = await userManager.GetRolesAsync(user);
            // 4) لو Student → هات StudentProfile
            StudentProfileDto? profile = null;

            if (roles.Contains("Student"))
            {
                profile = await _studentService.GetStudentProfileAsync(user.Id);
            }
            InstructorResponseDto? instructorProfile = null;

            if (roles.Contains("Instructor"))
            {
                instructorProfile = await _instructorService.GetByUserIdProfilrAsync(user.Id);
            }

            var token = await _tokenService.GenerateTokenAsync(user);

            // 6) نرجّع الرد
            return Ok(new AuthResponseDto
            {
                Success = true,
                Message = "Login successful",
                Data = profile,   // للـ Instructor هتبقى null عادي
                Token = token
            });
        }
           
        

        [Authorize]
        [HttpGet("profile")]
        public async Task<ActionResult<StudentProfileDto>> GetMyProfile()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

           
                var profile = await _studentService.GetStudentProfileAsync(userId);
            if (profile == null)
                return NotFound(new { message = "Student profile not found for this user" });
            return Ok(profile);
           
           
        }
        [Authorize(Roles = "Instructor")]
        [HttpGet("instructor-profile")]
        public async Task<ActionResult<InstructorResponseDto>> GetMyInstructorProfile(
    [FromServices] IInstructorService instructorService)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var instructor = await instructorService.GetByUserIdProfilrAsync(userId);
            if (instructor == null)
                return NotFound(new { message = "Instructor profile not found" });

            return Ok(instructor);
        }







        //[HttpPost("register")]
        //public async Task<IActionResult> Registration(RegisterUserDto userDto)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        //usrManager
        //        ApplicationUser user = new ApplicationUser();
        //        user.UserName = userDto.UserName;
        //        user.Email = userDto.Email;
        //        IdentityResult result = await userManager.CreateAsync(user, userDto.Password);
        //        if (result.Succeeded)
        //        {
        //            return Ok(user);
        //        }
        //        return BadRequest(result.Errors.FirstOrDefault());
        //    }
        //    return BadRequest(ModelState);
        //}

        //[HttpPost("Login")]
        //public async Task<IActionResult> Login(LoginUserDto userDto)
        //{

        //    if (ModelState.IsValid == true)
        //    {
        //        //1- check - create token
        //        ApplicationUser user = await userManager.FindByNameAsync(userDto.UserName);
        //        if (user != null)//2- user name found
        //        {
        //            bool found = await userManager.CheckPasswordAsync(user, userDto.Password);
        //            if (found)
        //            {
        //                //4- Claims Token
        //                var claims = new List<Claim>();
        //                claims.Add(new Claim(ClaimTypes.Name, user.UserName));
        //                claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
        //                claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

        //                //5- get role
        //                var roles = await userManager.GetRolesAsync(user);
        //                foreach (var itemRole in roles)
        //                {
        //                    claims.Add(new Claim(ClaimTypes.Role, itemRole));
        //                }

        //                //7- Security Key
        //                SecurityKey securityKey =
        //                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Secret"]));

        //                //6- Signing Credentials
        //                SigningCredentials signincred =
        //                    new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        //                //3- Create token
        //                JwtSecurityToken mytoken = new JwtSecurityToken(
        //                    issuer: config["JWT:ValidIssuer"],//url web api
        //                    audience: config["JWT:ValidAudiance"],//url consumer angular
        //                    claims: claims,
        //                    expires: DateTime.Now.AddHours(1),
        //                    signingCredentials: signincred
        //                    );
        //                return Ok(new
        //                {
        //                    token = new JwtSecurityTokenHandler().WriteToken(mytoken),
        //                    expiration = mytoken.ValidTo
        //                });
        //            }
        //        }
        //        return Unauthorized();

        //    }
        //    return Unauthorized();
        //}
    }
}
