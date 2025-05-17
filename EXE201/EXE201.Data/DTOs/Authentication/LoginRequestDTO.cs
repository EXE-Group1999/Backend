
using System.ComponentModel.DataAnnotations;


namespace EXE201.Data.DTOs.Authentication
{
    public class LoginRequestDTO
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty.ToString();
    }
}
