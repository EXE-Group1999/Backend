

using EXE201.Data;
using EXE201.Data.Entities;
using EXE201.Repository.Interfaces;

namespace EXE201.Repository.Repositories
{
    public class FurnitureRepository : Repository<Furniture>, IFurnitureRepository
    {
        private readonly FurnitureStoreDbContext _dbcontext;
        public FurnitureRepository(FurnitureStoreDbContext context) : base(context)
        {
            _dbcontext = context;
        }
    }
}
