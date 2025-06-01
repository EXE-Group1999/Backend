using EXE201.Data.DTOs;
using EXE201.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EXE201.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        /// <summary>
        /// Create a new review.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ReviewDto>> Create([FromBody] CreateReviewDto dto)
        {
            var createdReview = await _reviewService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetByFurnitureId), new { furnitureId = createdReview.FurnitureId }, createdReview);
        }

        /// <summary>
        /// Delete a review by its ID.
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _reviewService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException knf)
            {
                return NotFound(knf.Message);
            }
        }

        /// <summary>
        /// Get all reviews for a specific furniture item.
        /// </summary>
        [HttpGet("furniture/{furnitureId:int}")]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetByFurnitureId(int furnitureId)
        {
            var reviews = await _reviewService.GetReviewsByFurnitureIdAsync(furnitureId);
            return Ok(reviews);
        }
    }
}
