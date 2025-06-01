using EXE201.Data.DTOs;
using EXE201.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EXE201.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/users?pageIndex=1&pageSize=10&search=abc
        [HttpGet]
        public async Task<ActionResult<PaginatedResult<ApplicationUserDto>>> GetUsers([FromQuery] UserQueryParameters parameters)
        {
            var result = await _userService.GetAllAsync(parameters);
            return Ok(result);
        }

        // GET: api/users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationUserDto>> GetUser(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        // PUT: api/users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] ApplicationUserDto userDto)
        {
            if (id != userDto.Id)
                return BadRequest("User ID mismatch.");

            try
            {
                await _userService.UpdateAsync(userDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteAsync(id);
            return NoContent();
        }
    }
}
