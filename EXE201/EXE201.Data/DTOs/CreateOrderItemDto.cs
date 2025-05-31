

namespace EXE201.Data.DTOs
{
    public class CreateOrderItemDto
    {
        public int FurnitureId { get; set; }
        public int Quantity { get; set; }
        public decimal? CustomHeight { get; set; }
        public decimal? CustomWidth { get; set; }
        public decimal? CustomLength { get; set; }
    }
}
