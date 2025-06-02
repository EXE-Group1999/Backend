using EXE201.Data.DTOs;
using EXE201.Data.Entities;
using EXE201.Repository.Interfaces;
using EXE201.Service.Interface;


namespace EXE201.Service.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IOrderItemRepository _orderItemRepo;
        private readonly IShippingDetailRepository _shippingDetailRepo;
        private readonly IRepository<Payment> _paymentRepo;

        public OrderService(
            IOrderRepository orderRepo,
            IOrderItemRepository orderItemRepo,
            IShippingDetailRepository shippingDetailRepo,
            IRepository<Payment> paymentRepo)
        {
            _orderRepo = orderRepo;
            _orderItemRepo = orderItemRepo;
            _shippingDetailRepo = shippingDetailRepo;
            _paymentRepo = paymentRepo;
        }

        public async Task<OrderDto> CreateAsync(OrderDto dto)
        {
            var order = new Order
            {
                UserId = dto.UserId,
                OrderDate = DateTime.UtcNow,
                Status = "Pending",
                TotalAmount = dto.TotalAmount
            };

            await _orderRepo.AddAsync(order);

            foreach (var item in dto.OrderItems)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    FurnitureId = item.FurnitureId,
                    Quantity = item.Quantity,
                    CustomHeight = item.CustomHeight,
                    CustomWidth = item.CustomWidth,
                    CustomLength = item.CustomLength,
                    UnitPrice = item.UnitPrice
                };
                await _orderItemRepo.AddAsync(orderItem);
            }

            var shipping = new ShippingDetail
            {
                OrderId = order.Id,
                Address = dto.ShippingDetail.Address,
                PhoneNumber = dto.ShippingDetail.PhoneNumber,
                FullName = dto.ShippingDetail.FullName,
                City = dto.ShippingDetail.City,
                PostalCode = dto.ShippingDetail.PostalCode,
            };
            await _shippingDetailRepo.AddAsync(shipping);

            //var payment = new Payment
            //{
            //    OrderId = order.Id,
            //    PaymentMethod = dto.Payment.PaymentMethod,
            //    PaymentDate = DateTime.Now
            //};
            //await _paymentRepo.AddAsync(payment);

            return await GetByIdAsync(order.Id); // Return full dto
        }

        public async Task<PaginatedResult<OrderDto>> GetAllAsync(OrderQueryParameters parameters)
        {
            var orders = await _orderRepo.GetAllAsync(
                filter: o =>
                    (parameters.Status == null || o.Status == parameters.Status) &&
                    (parameters.UserId == null || o.UserId == parameters.UserId),
                includeProperties: "OrderItems.Furniture,ShippingDetail,Payment");


            var totalCount = orders.Count();
            var pagedOrders = orders
                .Skip((parameters.PageIndex - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToList();

            var result = new PaginatedResult<OrderDto>
            {
                Items = pagedOrders.Select(MapToDto).ToList(),
                TotalCount = totalCount,
                PageIndex = parameters.PageIndex,
                PageSize = parameters.PageSize
            };

            return result;
        }

        public async Task<OrderDto> GetByIdAsync(int id)
        {
            var order = await _orderRepo.GetAsync(
                o => o.Id == id,
                includeProperties: "User,OrderItems.Furniture,ShippingDetail,Payment"
            );

            if (order == null)
                return null;

            var orderDto = new OrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                ShippingDetail = new ShippingDetailDto
                {
                    Address = order.ShippingDetail?.Address,
                    PhoneNumber = order.ShippingDetail?.PhoneNumber,
                    FullName = order.ShippingDetail?.FullName,
                    City = order.ShippingDetail?.City,
                    PostalCode = order.ShippingDetail?.PostalCode
                },
                Payment = order.Payment == null ? null : new PaymentDto
                {
                    PaymentMethod = order.Payment.PaymentMethod,
                    PaymentDate = order.Payment.PaymentDate
                },
                OrderItems = order.OrderItems?.Select(item => new OrderItemDto
                {
                    FurnitureId = item.FurnitureId,
                    Quantity = item.Quantity,
                    CustomHeight = item.CustomHeight,
                    CustomWidth = item.CustomWidth,
                    CustomLength = item.CustomLength,
                    UnitPrice = item.UnitPrice,
                    SubTotal = item.Quantity * item.UnitPrice
                     
                }).ToList()
            };

            return orderDto;
        }
        public async Task<List<OrderDto>> GetOrdersByUserIdAsync(int userId)
        {
            var orders = await _orderRepo.GetAllAsync(
                filter: o => o.UserId == userId,
                includeProperties: "OrderItems.Furniture,ShippingDetail,Payment");

            return orders.Select(MapToDto).ToList();
        }

        public async Task UpdateStatusAsync(int id, string status)
        {
            var order = await _orderRepo.GetAsync(o => o.Id == id);
            if (order == null)
            {
                throw new Exception("Order not found");
            }

            order.Status = status;
            await _orderRepo.UpdateAsync(order);
        }

        private OrderDto MapToDto(Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                OrderItems = order.OrderItems?.Select(oi => new OrderItemDto
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
                    Address = order.ShippingDetail.Address,
                    PhoneNumber = order.ShippingDetail.PhoneNumber,
                    FullName = order.ShippingDetail.FullName
                },
                Payment = order.Payment == null ? null : new PaymentDto
                {
                    PaymentMethod = order.Payment.PaymentMethod,
                    AmountPaid = order.Payment.AmountPaid
                }
            };
        }
    }
}
