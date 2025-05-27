

namespace EXE201.Data.DTOs
{
    public class UserQueryParameters
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Search { get; set; }
    }
}
