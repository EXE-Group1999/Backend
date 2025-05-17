
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
            

            return service;
        }
    }
}
