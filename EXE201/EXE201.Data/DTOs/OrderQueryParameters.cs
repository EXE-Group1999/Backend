

namespace EXE201.Data.DTOs
{
    public class OrderQueryParameters
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Status { get; set; }
        public int? UserId { get; set; }
    }
}
