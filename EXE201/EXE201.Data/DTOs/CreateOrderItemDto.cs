using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201.Data.DTOs
{
    public class CreateOrderItemDto
    {
        public int FurnitureId { get; set; }
        public int Quantity { get; set; }
        public decimal? CustomHeight { get; set; }
        public decimal? CustomWidth { get; set; }
        public decimal? CustomLength { get; set; }
    }
}
