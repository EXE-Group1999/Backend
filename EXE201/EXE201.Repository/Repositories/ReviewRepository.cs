
using EXE201.Data;
using EXE201.Data.Entities;
using EXE201.Repository.Interfaces;

namespace EXE201.Repository.Repositories
{
    public class ReviewRepository : Repository<Review>, IReviewRepository
    {
        private readonly FurnitureStoreDbContext _dbcontext;
        public ReviewRepository(FurnitureStoreDbContext context) : base(context)
        {
            _dbcontext = context;
        }
    }
}
