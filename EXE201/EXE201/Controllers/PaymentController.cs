using EXE201.Data.DTOs;
using EXE201.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EXE201.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("create-link/{orderId}")]
        public async Task<IActionResult> CreateLink(long orderId)
        {
            var url = await _paymentService.CreatePaymentLinkAsync(orderId);
            return Ok(new { checkoutUrl = url });
        }
    }
}
