using EXE201.Data.Entities;


namespace EXE201.Service.Interface
{
    public interface IReviewService
    {
        Task<IEnumerable<Review>> GetReviewsByFurnitureIdAsync(int furnitureId);
        Task<Review> CreateAsync(Review review);
        Task DeleteAsync(int id);
    }
}
