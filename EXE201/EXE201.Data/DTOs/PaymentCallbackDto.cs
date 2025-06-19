using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201.Data.DTOs
{
    public class PayOSWebhookData
    {
        public int orderCode { get; set; }
        public string status { get; set; }
        public string? description { get; set; }
        public decimal amount { get; set; }
        public string paymentMethod { get; set; }
        public DateTime? paidAt { get; set; }
    }

    public class PayOSWebhookPayload
    {
        public string code { get; set; }
        public string desc { get; set; }
        public bool success { get; set; }
        public PayOSWebhookData data { get; set; }
        public string signature { get; set; }
    }
}
