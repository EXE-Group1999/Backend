using EXE201.Data.Entities;
using EXE201.Data.DTOs;


namespace EXE201.Service.Interface
{
    public interface IOrderService
    {
        Task<PaginatedResult<Order>> GetAllAsync(OrderQueryParameters parameters);
        Task<Order> GetByIdAsync(int id);
        Task<Order> CreateAsync(Order order);
        Task UpdateStatusAsync(int id, string status);
    }
}
