using EXE201.Data.DTOs;



namespace EXE201.Service.Interface
{
    public interface IReviewService
    {
        Task<ReviewDto> CreateAsync(CreateReviewDto dto);
        Task DeleteAsync(int id);
        Task<IEnumerable<ReviewDto>> GetReviewsByFurnitureIdAsync(int furnitureId);
    }
}
