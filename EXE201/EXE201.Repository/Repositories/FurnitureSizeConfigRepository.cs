

using EXE201.Data;
using EXE201.Data.Entities;
using EXE201.Repository.Interfaces;

namespace EXE201.Repository.Repositories
{
    public class FurnitureSizeConfigRepository : Repository<FurnitureSizeConfig>, IFurnitureSizeConfigRepository
    {
        private readonly FurnitureStoreDbContext _dbcontext;
        public FurnitureSizeConfigRepository(FurnitureStoreDbContext context) : base(context)
        {
            _dbcontext = context;
        }
    }
}
