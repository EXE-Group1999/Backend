using EXE201.Data.DTOs;
using EXE201.Repository.Interfaces;
using EXE201.Service.Interface;

namespace EXE201.Service.Services
{
    public class AdminService : IAdminService
    {
        private readonly IApplicationUserRepository _userRepo;
        private readonly IFurnitureRepository _furnitureRepo;
        private readonly IOrderRepository _orderRepo;
        private readonly IOrderItemRepository _orderItemRepo;

        public AdminService(
            IApplicationUserRepository userRepo,
            IFurnitureRepository furnitureRepo,
            IOrderRepository orderRepo,
            IOrderItemRepository orderItemRepo)
        {
            _userRepo = userRepo;
            _furnitureRepo = furnitureRepo;
            _orderRepo = orderRepo;
            _orderItemRepo = orderItemRepo;
        }

        /// <summary>
        /// Returns the dashboard statistics such as total users, orders, revenue, and product count.
        /// </summary>
        public async Task<DashboardStats> GetDashboardStatsAsync()
        {
            var users = await _userRepo.GetAllAsync();
            var orders = await _orderRepo.GetAllAsync();
            var furnitureList = await _furnitureRepo.GetAllAsync();

            var completedOrders = orders.Where(o => o.Status == "Completed");

            var totalRevenue = completedOrders.Sum(o => o.TotalAmount);

            return new DashboardStats
            {
                TotalUsers = users.Count(),
                TotalOrders = orders.Count(),
                TotalRevenue = totalRevenue,
                TotalProducts = furnitureList.Count()
            };
        }

        /// <summary>
        /// Returns the top 5 best-selling furniture items based on quantity sold.
        /// </summary>
        public async Task<IEnumerable<TopSellingFurniture>> GetTopSellingFurnitureAsync()
        {
            var orderItems = await _orderItemRepo.GetAllAsync(includeProperties: "Furniture,Order");

            var topSelling = orderItems
                .Where(oi => oi.Order != null && oi.Order.Status == "Completed" && oi.Furniture != null)
                .GroupBy(oi => oi.Furniture)
                .Select(group => new TopSellingFurniture
                {
                    FurnitureId = group.Key.Id,
                    FurnitureName = group.Key.Name,
                    TotalSold = group.Sum(oi => oi.Quantity),
                    RevenueGenerated = group.Sum(oi => oi.Quantity * oi.UnitPrice)
                })
                .OrderByDescending(f => f.TotalSold)
                .Take(5)
                .ToList();

            return topSelling;
        }

    }
}
