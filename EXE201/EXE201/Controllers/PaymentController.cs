using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using VNPAY.NET;
using Microsoft.AspNetCore.Http;
using System;
using VNPAY.NET.Enums;
using VNPAY.NET.Models;

namespace EXE201.Controllers
{
    [Route("payment")]
    public class PaymentController : Controller
    {
        private readonly IVnpay _vnpay;

        public PaymentController(IVnpay vnpay, IConfiguration config)
        {
            _vnpay = vnpay;
            _vnpay.Initialize(
                config["Vnpay:TmnCode"],
                config["Vnpay:HashSecret"],
                config["Vnpay:BaseUrl"],
                config["Vnpay:CallbackUrl"]
            );
        }

        // View trang thanh toán (tùy bạn có dùng hay không)
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        // Tạo URL thanh toán và trả lại client
        [HttpGet("create")]
        public IActionResult CreatePaymentUrl(double amount, string description)
        {
            try
            {
                var ip = HttpContext.Connection.RemoteIpAddress?.ToString();

                var request = new PaymentRequest
                {
                    PaymentId = DateTime.Now.Ticks,
                    Money = amount,
                    Description = description,
                    IpAddress = ip,
                    BankCode = BankCode.ANY,
                    CreatedDate = DateTime.Now,
                    Currency = Currency.VND,
                    Language = DisplayLanguage.Vietnamese
                };

                var url = _vnpay.GetPaymentUrl(request);

                return Redirect(url); // Hoặc return Created(url, url) nếu chỉ cần link
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Callback: Khi người dùng thanh toán xong sẽ chuyển hướng về đây
        [HttpGet("callback")]
        public IActionResult Callback()
        {
            try
            {
                if (Request.QueryString.HasValue)
                {
                    var result = _vnpay.GetPaymentResult(Request.Query);
                    var message = $"{result.PaymentResponse.Description}. {result.TransactionStatus.Description}";

                    return result.IsSuccess ? Ok(message) : BadRequest(message);
                }

                return NotFound("Không tìm thấy thông tin giao dịch.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // IPN: VNPAY sẽ tự động gọi endpoint này (nên dùng để xử lý cập nhật đơn hàng)
        [HttpGet("ipn")]
        public IActionResult IpnAction()
        {
            try
            {
                if (Request.QueryString.HasValue)
                {
                    var result = _vnpay.GetPaymentResult(Request.Query);

                    if (result.IsSuccess)
                    {
                        // TODO: Cập nhật trạng thái đơn hàng trong DB
                        return Ok();
                    }

                    // TODO: Xử lý đơn hàng thất bại nếu cần
                    return BadRequest("Thanh toán thất bại");
                }

                return NotFound("Không có thông tin từ VNPAY.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
