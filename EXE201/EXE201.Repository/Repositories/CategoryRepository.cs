

using EXE201.Data;
using EXE201.Data.Entities;
using EXE201.Repository.Interfaces;

namespace EXE201.Repository.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly FurnitureStoreDbContext _dbcontext;
        public CategoryRepository(FurnitureStoreDbContext context) : base(context)
        {
            _dbcontext = context;
        }
    }
}
