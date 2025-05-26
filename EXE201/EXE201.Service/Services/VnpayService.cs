using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using VNPAY.NET.Enums;
using VNPAY.NET.Models;
using VNPAY.NET;

namespace EXE201.Service.Services
{
    public class VnpayService
    {
        private readonly IVnpay _vnpay;

        public VnpayService(IConfiguration configuration, IVnpay vnpay)
        {
            _vnpay = vnpay;

            var tmnCode = configuration["Vnpay:TmnCode"];
            var hashSecret = configuration["Vnpay:HashSecret"];
            var baseUrl = configuration["Vnpay:BaseUrl"];
            var returnUrl = configuration["Vnpay:ReturnUrl"];

            _vnpay.Initialize(tmnCode, hashSecret, baseUrl, returnUrl);
        }

        public string CreatePaymentUrl(double amount, string description, string ip)
        {
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

            return _vnpay.GetPaymentUrl(request);
        }

        public PaymentResult GetPaymentResult(IQueryCollection query)
        {
            return _vnpay.GetPaymentResult(query);
        }
    }
}
