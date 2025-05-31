

namespace EXE201.Data.DTOs
{
    public class OrderItemDto
    {
        public int FurnitureId { get; set; }
        public int Quantity { get; set; }
        public decimal? CustomHeight { get; set; }
        public decimal? CustomWidth { get; set; }
        public decimal? CustomLength { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal { get; set; }
    }
}
