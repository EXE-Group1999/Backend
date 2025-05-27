using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201.Data.DTOs
{
    public class CreateFurnitureDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal BasePrice { get; set; }
        public int CategoryId { get; set; }
        public string? ImageUrl { get; set; }
        public string? Material { get; set; }
        public string? Color { get; set; }
        public FurnitureSizeConfigDto SizeConfig { get; set; }
    }
}
