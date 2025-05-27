
using EXE201.Service.Interface;
using EXE201.Service.Services;
using Microsoft.Extensions.DependencyInjection;


namespace EXE201.Service
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection service)
        {
            service.AddTransient<IAuthService, AuthService>();
            service.AddTransient<IFurnitureService, FurnitureService>();
            return service;
        }
    }
}
