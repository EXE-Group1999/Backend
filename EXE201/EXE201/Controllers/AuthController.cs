using EXE201.Data.DTOs.Authentication;
using EXE201.Data.Entities;
using EXE201.Service.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EXE201.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        //private readonly IUserService _userService;

        private readonly UserManager<ApplicationUser> _userManager;
        public AuthController(IAuthService authService, UserManager<ApplicationUser> userManager)
        {
            _authService = authService;
            _userManager = userManager;
            //_userService = userService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return Unauthorized("Invalid credentials.");
            }
            // Check if the account is soft-deleted
            //if (user.Status == "Deleted")
            //{
            //    return Unauthorized("Your account has been deleted. Please contact the admin!");
            //}

            var token = _authService.GenerateToken(user);
            return Ok(new
            {
                Token = token,
                userId = user.Id
            });
        }
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDTO dto)
        {
            if (dto == null)
                return BadRequest(new { Message = "Invalid user data." });

            try
            {
                var response = await _authService.RegisterUserAsync(dto);
                return Ok(new
                {
                    User = response.User,
                    Token = response.Token,
                    userId = response.UserId
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while register the user.", Error = ex.Message });
            }
        }
    }
}
