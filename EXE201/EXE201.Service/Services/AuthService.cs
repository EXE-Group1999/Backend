
using EXE201.Data.DTOs.Authentication;
using EXE201.Data.Entities;
using EXE201.Service.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EXE201.Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        public AuthService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;

        }
        public async Task<RegisterResponseDTO> RegisterUserAsync(RegisterUserDTO dto)
        {
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
            {
                throw new Exception("Email is already in use.");
            }
            var user = new ApplicationUser
            {
                UserName = dto.UserName,
                FullName = dto.FullName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                
            };

            // await _userRepository.AddAsync(user);
            // Hash password và tạo user
            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                throw new Exception(string.Join("; ", result.Errors.Select(e => e.Description)));
            }

            // Gán role cho user (nếu có)
            await _userManager.AddToRoleAsync(user, "CUSTOMER");

            var token = GenerateToken(user);
            return new RegisterResponseDTO
            {
                User = MapToDTO(user),
                Token = await token,
                UserId = user.Id
            };
        }
        private static UserDTO MapToDTO(ApplicationUser user)
        {
            return new UserDTO
            {
                Id = user.Id,
                FullName = user.FullName,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
            };
        }
        //public IActionResult GoogleLogin()
        //{
        //    var redirectUrl = "/api/v1/auth/google-response";
        //    var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
        //    return new ChallengeResult(GoogleDefaults.AuthenticationScheme, properties);
        //}

        //public async Task<IActionResult> GoogleResponse(HttpContext httpContext)
        //{
        //    var result = await httpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
        //    if (!result.Succeeded) return new BadRequestObjectResult("Google authentication failed.");

        //    var claims = result.Principal.Identities.FirstOrDefault()?.Claims;
        //    var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        //    var name = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

        //    if (string.IsNullOrEmpty(email))
        //        return new BadRequestObjectResult("Email not found.");

        //    // Find or create the user
        //    var user = await _userManager.FindByEmailAsync(email);
        //    if (user == null)
        //    {

        //        user = new ApplicationUser // Use your custom User class
        //        {
        //            UserName = email,
        //            Email = email,
        //            EmailConfirmed = true, // Assuming Google email is verified
        //        };

        //        var createResult = await _userManager.CreateAsync(user);
        //        if (!createResult.Succeeded)
        //        {
        //            return new BadRequestObjectResult(new { Errors = createResult.Errors.Select(e => e.Description) });
        //        }

        //        // Assign the existing "USER" role (assumes it exists with Name = "USER")
        //        var addRoleResult = await _userManager.AddToRoleAsync(user, "USER");
        //        if (!addRoleResult.Succeeded)
        //        {
        //            return new BadRequestObjectResult(new { Errors = addRoleResult.Errors.Select(e => e.Description) });
        //        }
        //    }

        //    // Generate JWT token
        //    var token = GenerateToken(user);

        //    return new OkObjectResult(new { Email = email, Name = name, Token = token });
        //}

        public async Task<string> GenerateToken(ApplicationUser user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),


        };
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
