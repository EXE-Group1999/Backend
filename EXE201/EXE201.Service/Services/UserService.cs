using EXE201.Data.DTOs;
using EXE201.Service.Interface;
using EXE201.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EXE201.Service.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
        }

        public async Task<PaginatedResult<ApplicationUserDto>> GetAllAsync(UserQueryParameters parameters)
        {
            var query = _userManager.Users.AsQueryable();

            if (!string.IsNullOrEmpty(parameters.Search))
            {
                query = query.Where(u =>
                    u.UserName.Contains(parameters.Search, StringComparison.OrdinalIgnoreCase) ||
                    u.Email.Contains(parameters.Search, StringComparison.OrdinalIgnoreCase));
            }

            var totalCount = await query.CountAsync();

            var pagedUsers = await query
                .Skip((parameters.PageIndex - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            var userDtos = pagedUsers.Select(u => new ApplicationUserDto
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                DateCreated = u.DateCreated,

            }).ToList();

            return new PaginatedResult<ApplicationUserDto>
            {
                Items = userDtos,
                TotalCount = totalCount,
                PageIndex = parameters.PageIndex,
                PageSize = parameters.PageSize
            };
        }

        public async Task<ApplicationUserDto> GetByIdAsync(int id)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return null;
            }

            return new ApplicationUserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                DateCreated = user.DateCreated,
            };
        }

        public async Task UpdateAsync(ApplicationUserDto userDto)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userDto.Id);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            user.FullName = userDto.FullName;
            user.Email = userDto.Email;


            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new Exception("Failed to update user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
    }
}
