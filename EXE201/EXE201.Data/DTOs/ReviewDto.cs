﻿

namespace EXE201.Data.DTOs
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public int FurnitureId { get; set; }
        public int UserId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime ReviewDate { get; set; }
    }
}
