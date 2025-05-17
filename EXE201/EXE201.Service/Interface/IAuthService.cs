

using EXE201.Data.DTOs.Authentication;
using EXE201.Data.Entities;

namespace EXE201.Service.Interface
{
    public interface IAuthService
    {
        Task<string> GenerateToken(ApplicationUser user);
        Task<RegisterResponseDTO> RegisterUserAsync(RegisterUserDTO dto);
        //IActionResult GoogleLogin();
        //Task<IActionResult> GoogleResponse(HttpContext httpContext);
    }
}
