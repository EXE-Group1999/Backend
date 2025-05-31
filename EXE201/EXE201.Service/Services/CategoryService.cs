using EXE201.Data.DTOs;
using EXE201.Data.Entities;
using EXE201.Repository.Interfaces;
using EXE201.Service.Interface;


namespace EXE201.Service.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepo;

        public CategoryService(ICategoryRepository categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        public async Task<CategoryDto> CreateAsync(CreateCategoryDto createCategoryDto)
        {
            var category = new Category
            {
                Name = createCategoryDto.Name,
                Description = createCategoryDto.Description
            };

            await _categoryRepo.AddAsync(category);

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                FurnitureCount = 0
            };
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _categoryRepo.GetAsync(c => c.Id == id);
            if (existing == null)
                throw new KeyNotFoundException($"Category with ID {id} not found");

            await _categoryRepo.DeleteAsync(existing);
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            var categories = await _categoryRepo.GetAllAsync(includeProperties: "Furnitures");

            return categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                FurnitureCount = c.Furnitures?.Count ?? 0
            });
        }

        public async Task<CategoryDto> GetByIdAsync(int id)
        {
            var category = await _categoryRepo.GetAsync(c => c.Id == id, includeProperties: "Furnitures");
            if (category == null)
                throw new KeyNotFoundException($"Category with ID {id} not found");

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                FurnitureCount = category.Furnitures?.Count ?? 0
            };
        }

        public async Task UpdateAsync(UpdateCategoryDto updateCategoryDto)
        {
            var existing = await _categoryRepo.GetAsync(c => c.Id == updateCategoryDto.Id);
            if (existing == null)
                throw new KeyNotFoundException($"Category with ID {updateCategoryDto.Id} not found");

            existing.Name = updateCategoryDto.Name;
            existing.Description = updateCategoryDto.Description;

            await _categoryRepo.UpdateAsync(existing);
        }
    }
}
