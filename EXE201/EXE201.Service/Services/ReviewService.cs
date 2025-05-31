using EXE201.Data.DTOs;
using EXE201.Data.Entities;
using EXE201.Repository.Interfaces;
using EXE201.Service.Interface;


namespace EXE201.Service.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepo;

        public ReviewService(IReviewRepository reviewRepo)
        {
            _reviewRepo = reviewRepo;
        }

        public async Task<ReviewDto> CreateAsync(CreateReviewDto dto)
        {
            var entity = new Review
            {
                FurnitureId = dto.FurnitureId,
                UserId = dto.UserId,
                Rating = dto.Rating,
                Comment = dto.Comment,
                ReviewDate = DateTime.UtcNow
            };

            var created = await _reviewRepo.AddAsync(entity);

            return new ReviewDto
            {
                Id = created.Id,
                FurnitureId = created.FurnitureId,
                UserId = created.UserId,
                Rating = created.Rating,
                Comment = created.Comment,
                ReviewDate = created.ReviewDate
            };
        }

        public async Task DeleteAsync(int id)
        {
            var review = await _reviewRepo.GetAsync(r => r.Id == id);
            if (review == null)
                throw new KeyNotFoundException($"Review with ID {id} not found.");

            await _reviewRepo.DeleteAsync(review);
        }

        public async Task<IEnumerable<ReviewDto>> GetReviewsByFurnitureIdAsync(int furnitureId)
        {
            var reviews = await _reviewRepo.GetAllAsync(r => r.FurnitureId == furnitureId);

            return reviews.Select(r => new ReviewDto
            {
                Id = r.Id,
                FurnitureId = r.FurnitureId,
                UserId = r.UserId,
                Rating = r.Rating,
                Comment = r.Comment,
                ReviewDate = r.ReviewDate
            });
        }
    }
}
