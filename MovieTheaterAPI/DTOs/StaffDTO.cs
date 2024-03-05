using MovieTheaterAPI.Entities;
using System.ComponentModel.DataAnnotations;

namespace MovieTheaterAPI.DTOs
{
    public class StaffDTO : UserDTO
    {
        public int StaffRole { get; set; } 
        public ICollection<TicketDTO>? Tickets { get;  } = new List<TicketDTO>();
    }
}
