using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201.Data.DTOs
{
    public class FurnitureSizeConfigDto
    {
        public bool SupportsCustomSize { get; set; } = true;
        public decimal? DefaultHeight { get; set; }
        public decimal? DefaultWidth { get; set; }
        public decimal? DefaultLength { get; set; }
        public decimal? MaxHeight { get; set; }
        public decimal? MaxWidth { get; set; }
        public decimal? MaxLength { get; set; }
    }
}
