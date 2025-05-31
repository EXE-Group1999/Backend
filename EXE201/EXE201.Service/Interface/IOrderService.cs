using EXE201.Data.Entities;
using EXE201.Data.DTOs;


namespace EXE201.Service.Interface
{
    public interface IOrderService
    {
        Task<OrderDto> CreateAsync(OrderDto dto);
        Task<PaginatedResult<OrderDto>> GetAllAsync(OrderQueryParameters parameters);
        Task<OrderDto> GetByIdAsync(int id);
        Task UpdateStatusAsync(int id, string status);
    }
}
