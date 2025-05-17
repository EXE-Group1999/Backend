
using Microsoft.Extensions.DependencyInjection;


namespace EXE201.Service
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection service)
        {
            
            //service.AddTransient<IRoomService, RoomService>();
            return service;
        }
    }
}
