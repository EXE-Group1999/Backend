

namespace EXE201.Data.DTOs
{
    public class CreateReviewDto
    {
        public int FurnitureId { get; set; }
        public int UserId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
    }
}
