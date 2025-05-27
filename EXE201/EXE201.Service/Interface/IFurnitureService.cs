using EXE201.Data.Entities;
using EXE201.Data.DTOs;


namespace EXE201.Service.Interface
{
    public interface IFurnitureService
    {
        Task<FurnitureDto> CreateAsync(CreateFurnitureDto dto);
        Task DeleteAsync(int id);
        Task<PaginatedResult<FurnitureDto>> GetAllAsync(FurnitureQueryParameters parameters);
        Task<FurnitureDto> GetByIdAsync(int id);
        Task UpdateAsync(int id, CreateFurnitureDto dto);
    }
}
