using EXE201.Data.DTOs;
using EXE201.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EXE201.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        /// <summary>
        /// Get dashboard statistics including total users, orders, revenue, and products.
        /// </summary>
        [HttpGet("dashboard")]
        public async Task<ActionResult<DashboardStats>> GetDashboardStats()
        {
            var stats = await _adminService.GetDashboardStatsAsync();
            return Ok(stats);
        }

        /// <summary>
        /// Get top 5 best-selling furniture items.
        /// </summary>
        [HttpGet("top-selling")]
        public async Task<ActionResult<IEnumerable<TopSellingFurniture>>> GetTopSellingFurniture()
        {
            var topSelling = await _adminService.GetTopSellingFurnitureAsync();
            return Ok(topSelling);
        }
    }
}
