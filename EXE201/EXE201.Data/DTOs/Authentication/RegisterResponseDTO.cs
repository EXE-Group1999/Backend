

namespace EXE201.Data.DTOs.Authentication
{
    public class RegisterResponseDTO
    {
        public UserDTO User { get; set; }
        public string Token { get; set; }
        public int UserId { get; set; }
    }
}
