using EXE201.Data.DTOs;


namespace EXE201.Service.Interface
{
    public interface IOrderService
    {
        Task<(OrderDto, string)> CreateAsync(CreateOrderDto dto);
        Task<PaginatedResult<OrderDto>> GetAllAsync(OrderQueryParameters parameters);
        Task<OrderDto> GetByIdAsync(int id);
        Task<List<OrderDto>> GetOrdersByUserIdAsync(int userId);
        Task UpdateStatusAsync(int id, string status);
    }
}
