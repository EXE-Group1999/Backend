using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201.Data.DTOs
{
    public class PaymentCallbackDto
    {
        public int orderCode { get; set; }
        public string status { get; set; } = string.Empty;
        public string? description { get; set; }
    }
}
