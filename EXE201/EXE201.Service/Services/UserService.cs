using EXE201.Data.DTOs;
using EXE201.Repository.Interfaces;
using EXE201.Service.Interface;



namespace EXE201.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IApplicationUserRepository _userRepo;

        public UserService(IApplicationUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _userRepo.GetAsync(u => u.Id == id);
            if (user != null)
            {
                await _userRepo.DeleteAsync(user);
            }
            // else optionally handle not found
        }

        public async Task<PaginatedResult<ApplicationUserDto>> GetAllAsync(UserQueryParameters parameters)
        {
            var query = (await _userRepo.GetAllAsync())
                .AsQueryable();

            if (!string.IsNullOrEmpty(parameters.Search))
            {
                query = query.Where(u =>
                    u.UserName.Contains(parameters.Search, StringComparison.OrdinalIgnoreCase) ||
                    u.Email.Contains(parameters.Search, StringComparison.OrdinalIgnoreCase));
            }

            var totalCount = query.Count();

            var pagedUsers = query
                .Skip((parameters.PageIndex - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
            .ToList();

            var userDtos = pagedUsers.Select(u => new ApplicationUserDto
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email
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
            var user = await _userRepo.GetAsync(u => u.Id == id);
            if (user == null)
            {
                return null; // Or throw exception based on your error strategy
            }

            return new ApplicationUserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email
            };
        }

        public async Task UpdateAsync(ApplicationUserDto userDto)
        {
            var user = await _userRepo.GetAsync(u => u.Id == userDto.Id);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            user.FullName = userDto.FullName;
            user.Email = userDto.Email;
            

            // update other properties as needed

            await _userRepo.UpdateAsync(user);
        }
    }
}
