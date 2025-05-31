
using EXE201.Data.DTOs;


namespace EXE201.Service.Interface
{
    public interface IUserService
    {
        Task DeleteAsync(int id);
        Task<PaginatedResult<ApplicationUserDto>> GetAllAsync(UserQueryParameters parameters);
        Task<ApplicationUserDto> GetByIdAsync(int id);
        Task UpdateAsync(ApplicationUserDto userDto);
    }
}
