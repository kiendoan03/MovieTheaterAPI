using System.ComponentModel.DataAnnotations;

namespace MovieTheaterAPI.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Image { get; set; } = null!;
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;
        [MinLength(10), MaxLength(10)]
        public string PhoneNumber { get; set; } = null!;
        public string Username { get; set; } = null!;
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$",
        ErrorMessage = "Password must contain at least one lowercase letter, one uppercase letter, one digit, one special character, and be 8-15 characters long.")]
        public string PasswordHash { get; set; } = null!;
        [DataType(DataType.Date)]
        public string DOB { get; set; }
        public string Address { get; set; } = null!;
        public string? Token { get; set; }
        public string? Role { get; set; }
    }
}
