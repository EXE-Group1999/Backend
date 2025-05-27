

using EXE201.Data.DTOs;

namespace EXE201.Service.Interface
{
    public interface IAdminService
    {
        Task<DashboardStats> GetDashboardStatsAsync();
        Task<IEnumerable<TopSellingFurniture>> GetTopSellingFurnitureAsync();
    }
}
