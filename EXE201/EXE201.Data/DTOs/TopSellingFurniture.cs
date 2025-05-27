using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201.Data.DTOs
{
    public class TopSellingFurniture
    {
        public int FurnitureId { get; set; }
        public string FurnitureName { get; set; }
        public int TotalSold { get; set; }
        public decimal RevenueGenerated { get; set; }
    }
}
