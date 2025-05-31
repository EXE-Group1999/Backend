using EXE201.Data.DTOs;
using EXE201.Data.Entities;
using EXE201.Repository.Interfaces;
using EXE201.Service.Interface;

namespace EXE201.Service.Services
{
    public class FurnitureService : IFurnitureService
    {
        private readonly IFurnitureRepository _repository;

        public FurnitureService(IFurnitureRepository furnitureRepository)
        {
            _repository = furnitureRepository;
        }

        public async Task<FurnitureDto> CreateAsync(CreateFurnitureDto dto)
        {
            var furniture = new Furniture
            {
                Name = dto.Name,
                Description = dto.Description,
                BasePrice = dto.BasePrice,
                CategoryId = dto.CategoryId,
                ImageUrl = dto.ImageUrl,
                Material = dto.Material,
                Color = dto.Color,
                DateAdded = DateTime.UtcNow,
                SizeConfig = new FurnitureSizeConfig
                {
                    SupportsCustomSize = dto.SizeConfig.SupportsCustomSize,
                    DefaultHeight = dto.SizeConfig.DefaultHeight,
                    DefaultWidth = dto.SizeConfig.DefaultWidth,
                    DefaultLength = dto.SizeConfig.DefaultLength,
                    MaxHeight = dto.SizeConfig.MaxHeight,
                    MaxWidth = dto.SizeConfig.MaxWidth,
                    MaxLength = dto.SizeConfig.MaxLength
                }
            };

            await _repository.AddAsync(furniture);
            return ToDto(furniture);
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _repository.GetAsync(f => f.Id == id, "SizeConfig");
            if (existing == null)
                throw new Exception($"Furniture with ID {id} not found");

            await _repository.DeleteAsync(existing);
        }

        public async Task<PaginatedResult<FurnitureDto>> GetAllAsync(FurnitureQueryParameters parameters)
        {
            var allItems = await _repository.GetAllAsync(null, "SizeConfig");

            var filtered = allItems.AsQueryable();

            if (!string.IsNullOrEmpty(parameters.Search))
            {
                filtered = filtered.Where(f =>
                    f.Name.Contains(parameters.Search) ||
                    f.Description.Contains(parameters.Search));
            }
            if(parameters.CategoryId.HasValue)
            {
                filtered = filtered.Where(f => f.CategoryId == parameters.CategoryId.Value);
            }

            var total = filtered.Count();
            var paged = filtered
                .Skip((parameters.PageIndex - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToList();

            return new PaginatedResult<FurnitureDto>
            {
                Items = paged.Select(ToDto),
                TotalCount = total,
                PageIndex = parameters.PageIndex,
                PageSize = parameters.PageSize
            };
        }

        public async Task<FurnitureDto> GetByIdAsync(int id)
        {
            var furniture = await _repository.GetAsync(f => f.Id == id, "SizeConfig");
            if (furniture == null)
                throw new Exception($"Furniture with ID {id} not found");

            return ToDto(furniture);
        }

        public async Task UpdateAsync(int id, CreateFurnitureDto dto)
        {
            var existing = await _repository.GetAsync(f => f.Id == id, "SizeConfig");
            if (existing == null)
                throw new Exception($"Furniture with ID {id} not found");

            existing.Name = dto.Name;
            existing.Description = dto.Description;
            existing.BasePrice = dto.BasePrice;
            existing.CategoryId = dto.CategoryId;
            existing.ImageUrl = dto.ImageUrl;
            existing.Material = dto.Material;
            existing.Color = dto.Color;

            if (existing.SizeConfig == null)
            {
                existing.SizeConfig = new FurnitureSizeConfig();
            }

            existing.SizeConfig.SupportsCustomSize = dto.SizeConfig.SupportsCustomSize;
            existing.SizeConfig.DefaultHeight = dto.SizeConfig.DefaultHeight;
            existing.SizeConfig.DefaultWidth = dto.SizeConfig.DefaultWidth;
            existing.SizeConfig.DefaultLength = dto.SizeConfig.DefaultLength;
            existing.SizeConfig.MaxHeight = dto.SizeConfig.MaxHeight;
            existing.SizeConfig.MaxWidth = dto.SizeConfig.MaxWidth;
            existing.SizeConfig.MaxLength = dto.SizeConfig.MaxLength;

            await _repository.UpdateAsync(existing);
        }

        // Helper method to map Furniture -> FurnitureDto
        private FurnitureDto ToDto(Furniture f)
        {
            return new FurnitureDto
            {
                Id = f.Id,
                Name = f.Name,
                Description = f.Description,
                BasePrice = f.BasePrice,
                CategoryId = f.CategoryId,
                ImageUrl = f.ImageUrl,
                Material = f.Material,
                Color = f.Color,
                DateAdded = f.DateAdded,
                SizeConfig = f.SizeConfig == null ? null : new FurnitureSizeConfigDto
                {
                    SupportsCustomSize = f.SizeConfig.SupportsCustomSize,
                    DefaultHeight = f.SizeConfig.DefaultHeight,
                    DefaultWidth = f.SizeConfig.DefaultWidth,
                    DefaultLength = f.SizeConfig.DefaultLength,
                    MaxHeight = f.SizeConfig.MaxHeight,
                    MaxWidth = f.SizeConfig.MaxWidth,
                    MaxLength = f.SizeConfig.MaxLength
                }
            };
        }
    }
}
