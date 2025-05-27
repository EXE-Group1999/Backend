using EXE201.Data.Entities;
using EXE201.Data.DTOs;

namespace EXE201.Service.Interface
{
    public interface IUserService
    {
        Task<PaginatedResult<ApplicationUser>> GetAllAsync(UserQueryParameters parameters);
        Task<ApplicationUser> GetByIdAsync(int id);
        Task UpdateAsync(ApplicationUser user);
        Task DeleteAsync(int id);
    }
}
