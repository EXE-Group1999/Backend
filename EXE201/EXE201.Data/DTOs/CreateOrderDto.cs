

namespace EXE201.Data.DTOs
{
    public class CreateOrderDto
    {
        public int UserId { get; set; }
        public List<CreateOrderItemDto> OrderItems { get; set; }
        public ShippingDetailDto ShippingDetail { get; set; }
        public PaymentDto Payment { get; set; }
    }
}
