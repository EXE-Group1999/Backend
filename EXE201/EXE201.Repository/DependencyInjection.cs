
using EXE201.Repository.Interfaces;
using EXE201.Repository.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace EXE201.Repository
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepositories(this IServiceCollection service)
        {
            service.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            service.AddTransient<IApplicationUserRepository, ApplicationUserRepository>();
            service.AddTransient<ICategoryRepository, CategoryRepository>();
            service.AddTransient<IFurnitureRepository, FurnitureRepository>();
            service.AddTransient<IFurnitureSizeConfigRepository, FurnitureSizeConfigRepository>();
            service.AddTransient<IOrderItemRepository, OrderItemRepository>();
            service.AddTransient<IOrderRepository, OrderRepository>();
            service.AddTransient<IReviewRepository, ReviewRepository>();
            service.AddTransient<IShippingDetailRepository, ShippingDetailRepository>();
            

            return service;
        }
    }
}
