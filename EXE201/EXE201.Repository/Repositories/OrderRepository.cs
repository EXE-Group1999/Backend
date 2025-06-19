

using EXE201.Data;
using EXE201.Data.DTOs;
using EXE201.Data.Entities;
using EXE201.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EXE201.Repository.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly FurnitureStoreDbContext _dbcontext;
        public OrderRepository(FurnitureStoreDbContext context) : base(context)
        {
            _dbcontext = context;
        }
        public async Task<Order> GetOrderWithItemsAsync(long orderId)
        {
            return await _dbcontext.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }
        public async Task<List<Order>> GetOrdersByUserIdAsync(int userId)
        {
            return await _dbcontext.Orders
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<Order> GetByIdAsync(int orderId)
        {
            return await _dbcontext.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<OrderDto> GetOrderDtoByIdAsync(int orderId)
        {
            var order = await _dbcontext.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Furniture)
                .Include(o => o.ShippingDetail)
                .Include(o => o.Payment)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                return null;

            return new OrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                OrderItems = order.OrderItems.Select(oi => new OrderItemDto
                {
                    FurnitureId = oi.FurnitureId,
                    Quantity = oi.Quantity,
                    CustomHeight = oi.CustomHeight,
                    CustomWidth = oi.CustomWidth,
                    CustomLength = oi.CustomLength,
                    UnitPrice = oi.UnitPrice,
                    SubTotal = oi.UnitPrice * oi.Quantity
                }).ToList(),
                ShippingDetail = order.ShippingDetail == null ? null : new ShippingDetailDto
                {
                    FullName = order.ShippingDetail.FullName,
                    Address = order.ShippingDetail.Address,
                    City = order.ShippingDetail.City,
                    PostalCode = order.ShippingDetail.PostalCode,
                    PhoneNumber = order.ShippingDetail.PhoneNumber
                },
                Payment = order.Payment == null ? null : new PaymentDto
                {
                    Id = order.Payment.Id,
                    OrderId = order.Id,
                    PaymentMethod = order.Payment.PaymentMethod,
                    PaymentDate = order.Payment.PaymentDate,
                    AmountPaid = order.Payment.AmountPaid
                }
            };
        }
    }
}
