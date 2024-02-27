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
        public string Phone { get; set; } = null!;
        public string Username { get; set; } = null!;
        [MinLength(8)]
        public string Password { get; set; } = null!;
        [DataType(DataType.Date)]
        public string DOB { get; set; }
        public string Address { get; set; } = null!;
        public string? Token { get; set; }
    }
}
