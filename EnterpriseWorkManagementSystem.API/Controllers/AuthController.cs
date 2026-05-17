using EnterpriseWorkManagementSystem.API.Models.Auth;
using EnterpriseWorkManagementSystem.Application.Abstractions.Infrastructure;
using EnterpriseWorkManagementSystem.Domain.Entities;
using EnterpriseWorkManagementSystem.Persistence.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace EnterpriseWorkManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;

        public AuthController(
            UserManager<AppUser> userManager,
            ITokenService tokenService,
            IConfiguration configuration,
            AppDbContext context)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _configuration = configuration;
            _context = context;
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
        public async Task<IActionResult> Login(LoginRequest request, CancellationToken cancellationToken)
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

            var refreshToken = _tokenService.CreateRefreshToken();

            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            };

            _context.RefreshTokens.Add(refreshTokenEntity);

            await _context.SaveChangesAsync(cancellationToken);

            var expirationMinutes = Convert.ToDouble(
                _configuration["JwtSettings:ExpirationInMinutes"]);

            return Ok(new AuthResponse
            {
                Token = token,
                RefreshToken = refreshToken,
                Expiration = DateTime.UtcNow.AddMinutes(expirationMinutes)
            });
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var existingRefreshToken = await _context.RefreshTokens
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Token == request.RefreshToken, cancellationToken);

            if (existingRefreshToken is null)
                return Unauthorized("Invalid refresh token.");

            if (existingRefreshToken.IsRevoked)
                return Unauthorized("Refresh token revoked.");

            if (existingRefreshToken.ExpiresAt < DateTime.UtcNow)
                return Unauthorized("Refresh token expired.");

            var user = existingRefreshToken.User;

            var roles = await _userManager.GetRolesAsync(user);

            var newAccessToken = _tokenService.CreateToken(user.Id, user.Email!, roles);
            var newRefreshToken = _tokenService.CreateRefreshToken();

            existingRefreshToken.IsRevoked = true;
            existingRefreshToken.UpdatedDate = DateTime.UtcNow;

            var newRefreshTokenEntity = new RefreshToken
            {
                Token = newRefreshToken,
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            };

            _context.RefreshTokens.Add(newRefreshTokenEntity);

            await _context.SaveChangesAsync(cancellationToken);

            var expirationMinutes = Convert.ToDouble(
                _configuration["JwtSettings:ExpirationInMinutes"]);

            return Ok(new AuthResponse
            {
                Token = newAccessToken,
                RefreshToken = newRefreshToken,
                Expiration = DateTime.UtcNow.AddMinutes(expirationMinutes)
            });
        }
    }
}
