using LarksWebApp.DTOs.DTO_User;
using LarksWebApp.Models.Users;
using LarksWebApp.Repository.TokenRepository;
using LarksWebApp.Services.Loggers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace LarksWebApp.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private ILoggerTrace _serviceLogger;
        private readonly IConfiguration _configuration;

        private static readonly TimeSpan AccessTokenLifetime = TimeSpan.FromHours(1);
        private static readonly TimeSpan RefreshTokenLifetime = TimeSpan.FromDays(30);

        public AuthenticateController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager,
            IRefreshTokenRepository refreshTokenRepository, ILoggerTrace serviceLogger, IConfiguration configuration)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _refreshTokenRepository = refreshTokenRepository;
            _serviceLogger = serviceLogger;
            _configuration = configuration;

        }


        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(LoginResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(loginModel.Username);

                if (user != null && await _userManager.CheckPasswordAsync(user, loginModel.Password))
                {
                    var userRoles = await _userManager.GetRolesAsync(user);

                    var (accessToken, expiration) = GenerateAccessToken(loginModel.Username, userRoles);
                    var refreshToken = await IssueRefreshTokenAsync(user.Id);

                    return Ok(new LoginResponseDTO
                    {
                        Username = loginModel.Username,
                        FullName = user.FirstName + " " + user.LastName,
                        Token = accessToken,
                        Expiration = expiration,
                        RefreshToken = refreshToken
                    });
                }

                return Unauthorized();
            }
            catch (Exception ex)
            {
                _serviceLogger.LogError("An error occurred while trying to login " + ex.Message);
                return BadRequest(new {errors = "Error while attempting to login"});
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("refresh")]
        [ProducesResponseType(typeof(LoginResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(SerializableError), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestDTO refreshTokenRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var existingToken = _refreshTokenRepository.GetByHash(HashToken(refreshTokenRequest.RefreshToken));

                if (existingToken == null || existingToken.IsRevoked || existingToken.ExpiresAt < DateTime.UtcNow)
                {
                    return Unauthorized();
                }

                var user = await _userManager.FindByIdAsync(existingToken.UserId);

                if (user == null)
                {
                    return Unauthorized();
                }

                // A refresh token is single-use: revoke the presented one and issue a new one.
                _refreshTokenRepository.Revoke(existingToken);

                var userRoles = await _userManager.GetRolesAsync(user);

                var (accessToken, expiration) = GenerateAccessToken(user.Email, userRoles);
                var newRefreshToken = await IssueRefreshTokenAsync(user.Id);

                return Ok(new LoginResponseDTO
                {
                    Username = user.Email,
                    FullName = user.FirstName + " " + user.LastName,
                    Token = accessToken,
                    Expiration = expiration,
                    RefreshToken = newRefreshToken
                });
            }
            catch (Exception ex)
            {
                _serviceLogger.LogError("An error occurred while trying to refresh a token " + ex.Message);
                return Unauthorized();
            }
        }

        private (string token, DateTime expiration) GenerateAccessToken(string username, IEnumerable<string> roles)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.Now.Add(AccessTokenLifetime),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return (new JwtSecurityTokenHandler().WriteToken(token), token.ValidTo);
        }

        private async Task<string> IssueRefreshTokenAsync(string userId)
        {
            var rawValue = GenerateRefreshTokenValue();

            await _refreshTokenRepository.CreateAsync(new RefreshToken
            {
                UserId = userId,
                TokenHash = HashToken(rawValue),
                ExpiresAt = DateTime.UtcNow.Add(RefreshTokenLifetime)
            });

            return rawValue;
        }

        private static string GenerateRefreshTokenValue()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        private static string HashToken(string value)
        {
            return Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(value)));
        }
    }
}
