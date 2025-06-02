

using EXE201.Data.Entities;

namespace EXE201.Repository.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<Order> GetOrderWithItemsAsync(long orderId);
        Task<List<Order>> GetOrdersByUserIdAsync(int userId);
        Task<Order> GetByIdAsync(int orderId);
    }
}
