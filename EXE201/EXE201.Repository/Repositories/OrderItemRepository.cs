

using EXE201.Data;
using EXE201.Data.Entities;
using EXE201.Repository.Interfaces;

namespace EXE201.Repository.Repositories
{
    public class OrderItemRepository : Repository<OrderItem>, IOrderItemRepository
    {
        private readonly FurnitureStoreDbContext _dbcontext;
        public OrderItemRepository(FurnitureStoreDbContext context) : base(context)
        {
            _dbcontext = context;
        }
    }
}
