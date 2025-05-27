

using EXE201.Data;
using EXE201.Data.Entities;
using EXE201.Repository.Interfaces;

namespace EXE201.Repository.Repositories
{
    public class ShippingDetailRepository : Repository<ShippingDetail>, IShippingDetailRepository
    {
        private readonly FurnitureStoreDbContext _dbcontext;
        public ShippingDetailRepository(FurnitureStoreDbContext context) : base(context)
        {
            _dbcontext = context;
        }
    }
}
