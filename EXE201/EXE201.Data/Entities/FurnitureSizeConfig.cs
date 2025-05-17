

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EXE201.Data.Entities
{
    public class FurnitureSizeConfig
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FurnitureId { get; set; }
        public bool SupportsCustomSize { get; set; } = true;
        public decimal? DefaultHeight { get; set; }
        public decimal? DefaultWidth { get; set; }
        public decimal? DefaultLength { get; set; }
        public decimal? MaxHeight { get; set; }
        public decimal? MaxWidth { get; set; }
        public decimal? MaxLength { get; set; }

        public Furniture Furniture { get; set; }
    }
}
