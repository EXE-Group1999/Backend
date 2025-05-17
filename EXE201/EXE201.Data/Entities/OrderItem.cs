

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EXE201.Data.Entities
{
    public class OrderItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int FurnitureId { get; set; }
        public int Quantity { get; set; }
        public decimal? CustomHeight { get; set; }
        public decimal? CustomWidth { get; set; }
        public decimal? CustomLength { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal { get; set; }

        public Order Order { get; set; }
        public Furniture Furniture { get; set; }
    }
}
