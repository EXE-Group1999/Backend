

using EXE201.Data;
using EXE201.Data.Entities;
using EXE201.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EXE201.Repository.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly FurnitureStoreDbContext _dbcontext;
        public OrderRepository(FurnitureStoreDbContext context) : base(context)
        {
            _dbcontext = context;
        }
        public async Task<Order> GetOrderWithItemsAsync(long orderId)
        {
            return await _dbcontext.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }
        public async Task<List<Order>> GetOrdersByUserIdAsync(int userId)
        {
            return await _dbcontext.Orders
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<Order> GetByIdAsync(int orderId)
        {
            return await _dbcontext.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
        }
    }
}
