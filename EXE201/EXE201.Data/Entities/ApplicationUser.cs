

using Microsoft.AspNetCore.Identity;

namespace EXE201.Data.Entities
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string FullName { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;

        public ICollection<Order> Orders { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}
