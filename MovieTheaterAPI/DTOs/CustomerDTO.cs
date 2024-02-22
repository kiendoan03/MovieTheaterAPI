using MovieTheaterAPI.Entities;
using System.ComponentModel.DataAnnotations;

namespace MovieTheaterAPI.DTOs
{
    public class CustomerDTO
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = null!;
        [DataType(DataType.EmailAddress)]
        public string CustomerEmail { get; set; } = null!;
        [MinLength(10), MaxLength(10)]
        public string CustomerPhone { get; set; } = null!;
        public string CustomerAddress { get; set; } = null!;
        public string CustomerUsername { get; set; } = null!;
        [MinLength(8)]
        public string CustomerPassword { get; set; } = null!;
        public string CustomerImage { get; set; } = null!;
        [DataType(DataType.Date)]
        public string? CustomerBirthdate { get; set; }
        public ICollection<TicketDTO>? Tickets { get;  } = new List<TicketDTO>();
    }
}
