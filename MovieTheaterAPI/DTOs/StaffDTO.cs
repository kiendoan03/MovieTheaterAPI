using MovieTheaterAPI.Entities;
using System.ComponentModel.DataAnnotations;

namespace MovieTheaterAPI.DTOs
{
    public class StaffDTO : UserDTO
    {
        public string StaffRole { get; set; } = null!;
        public ICollection<TicketDTO>? Tickets { get;  } = new List<TicketDTO>();
    }
}
