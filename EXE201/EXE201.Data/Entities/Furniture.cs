

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EXE201.Data.Entities
{
    [Table("Furniture")]
    public class Furniture
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal BasePrice { get; set; }
        public int CategoryId { get; set; }
        public string? ImageUrl { get; set; }
        public string? Material { get; set; }
        public string? Color { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.Now;

        public Category Category { get; set; }
        public FurnitureSizeConfig SizeConfig { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}
