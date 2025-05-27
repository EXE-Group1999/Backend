

using EXE201.Data;
using EXE201.Data.Entities;
using EXE201.Repository.Interfaces;

namespace EXE201.Repository.Repositories
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly FurnitureStoreDbContext _dbcontext;
        public ApplicationUserRepository(FurnitureStoreDbContext context) : base(context)
        {
            _dbcontext = context;
        }
    }
}
