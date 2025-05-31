using EXE201.Data.DTOs;



namespace EXE201.Service.Interface
{
    public interface ICategoryService
    {
        Task<CategoryDto> CreateAsync(CreateCategoryDto createCategoryDto);
        Task DeleteAsync(int id);
        Task<IEnumerable<CategoryDto>> GetAllAsync();
        Task<CategoryDto> GetByIdAsync(int id);
        Task UpdateAsync(UpdateCategoryDto updateCategoryDto);
    }
}
