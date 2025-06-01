using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201.Data.DTOs
{
    public class CreatePaymentRequest
    {
        public long OrderId { get; set; }
        //public int Amount { get; set; }
        //public string Description { get; set; } = string.Empty;
    }
}
