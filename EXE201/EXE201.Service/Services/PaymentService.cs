using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EXE201.Data.DTOs;
using EXE201.Data.Entities;
using EXE201.Repository.Interfaces;
using Net.payOS;
using Net.payOS.Types;

namespace EXE201.Service.Services
{
    public interface IPaymentService
    {
        Task<string> CreatePaymentLinkAsync(long orderId);
    }

    public class PaymentService : IPaymentService
    {
        private readonly PayOS _payOS;
        private readonly IOrderRepository _orderRepository;

        public PaymentService(PayOS payOS, IOrderRepository orderRepository)
        {
            _payOS = payOS;
            _orderRepository = orderRepository;
        }

        public async Task<string> CreatePaymentLinkAsync(long orderId)
        {
            var order = await _orderRepository.GetOrderWithItemsAsync(orderId);

            if (order == null)
            {
                throw new Exception($"Order with ID {orderId} not found.");
            }

            if (order.OrderItems == null || !order.OrderItems.Any())
            {
                throw new Exception("Order does not contain any items.");
            }

            var items = order.OrderItems.Select(item => new ItemData(
                name: $"Furniture #{item.FurnitureId}",
                quantity: item.Quantity,
                price: (int)item.UnitPrice
            )).ToList();

            var paymentData = new PaymentData(
                orderCode: order.Id,
                amount: (int)order.TotalAmount,
                description: $"Payment for Order #{order.Id}",
                items: items,
                cancelUrl: "https://yourdomain.com/payment/cancel",
                returnUrl: "https://yourdomain.com/payment/success"
            );

            var result = await _payOS.createPaymentLink(paymentData);

            return result.checkoutUrl;
        }
    }
}
