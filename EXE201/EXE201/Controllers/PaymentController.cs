using EXE201.Data;
using EXE201.Data.DTOs;
using EXE201.Data.Entities;
using EXE201.Repository.Interfaces;
using EXE201.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EXE201.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IOrderRepository _orderRepository;
        private readonly FurnitureStoreDbContext _context;
        public PaymentController(IPaymentService paymentService, IOrderRepository orderRepository, FurnitureStoreDbContext context)
        {
            _paymentService = paymentService;
            _orderRepository = orderRepository;
            _context = context;
        }

        [HttpPost("create-link/{orderId}")]
        public async Task<IActionResult> CreateLink(int orderId)
        {
            try
            {
                var url = await _paymentService.CreatePaymentLinkAsync(orderId);
                return Ok(new { checkoutUrl = url });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpPost("callback")]
        public async Task<IActionResult> HandlePaymentCallback([FromBody] PayOSWebhookPayload payload)
        {
            var data = payload.data;

            var order = await _orderRepository.GetOrderWithItemsAsync(data.orderCode);
            if (order == null)
                return NotFound();

            if (data.status == "PAID")
            {
                order.Status = "Confirmed";
                await _orderRepository.UpdateAsync(order);

                var payment = new Payment
                {
                    OrderId = order.Id,
                    AmountPaid = data.amount,
                    PaymentDate = data.paidAt ?? DateTime.UtcNow,
                    PaymentMethod = data.paymentMethod
                };
                await _context.Payments.AddAsync(payment);
                await _context.SaveChangesAsync();

                return Ok(new { success = true });
            }

            return BadRequest(new { message = "Invalid status" });
        }

        [HttpGet("success")]
        [AllowAnonymous]
        public IActionResult PaymentSuccess()
        {
            return Ok("Payment Success!");
        }


        [HttpGet("cancel")]
        [AllowAnonymous]
        public IActionResult PaymentCancel()
        {
            return Ok("Payment Cancel.");
        }

        [HttpGet("history/{userId}")]
        public async Task<IActionResult> GetPaymentHistoryByUserId(int userId)
        {
            // Lấy đơn hàng của user
            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);

            if (orders == null || !orders.Any())
                return Ok(new List<PaymentDto>());

            var orderIds = orders.Select(o => o.Id).ToList();

            // Truy vấn các payment theo danh sách orderId
            var payments = await _context.Payments
                .Where(p => orderIds.Contains(p.OrderId))
                .ToListAsync();

            if (!payments.Any())
                return Ok(new List<PaymentDto>());

            // Map sang DTO
            var result = payments.Select(p => new PaymentDto
            {
                Id = p.Id,
                AmountPaid = p.AmountPaid,
                PaymentDate = p.PaymentDate,
                OrderId = p.OrderId,
                PaymentMethod = p.PaymentMethod
            }).ToList();

            return Ok(result);
        }
    }
}
