using EnterpriseWorkManagementSystem.API.Models.Auth;
using EnterpriseWorkManagementSystem.Application.Abstractions.Infrastructure;
using EnterpriseWorkManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseWorkManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;

        public AuthController(
            UserManager<AppUser> userManager,
            ITokenService tokenService,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);

            if (existingUser is not null)
            {
                return BadRequest("User already exists.");
            }

            var user = new AppUser
            {
                FullName = request.FullName,
                UserName = request.Email,
                Email = request.Email
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            await _userManager.AddToRoleAsync(user, "Employee");

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.Select(x => x.Description));
            }

            return Ok("User registered successfully.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
            {
                return Unauthorized("Invalid email or password.");
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!isPasswordValid)
            {
                return Unauthorized("Invalid email or password.");
            }

            var roles = await _userManager.GetRolesAsync(user);

            var token = _tokenService.CreateToken(user.Id, user.Email!, roles);

            var expirationMinutes = Convert.ToDouble(
                _configuration["JwtSettings:ExpirationInMinutes"]);

            return Ok(new AuthResponse
            {
                Token = token,
                Expiration = DateTime.UtcNow.AddMinutes(expirationMinutes)
            });
        }
    }
}
