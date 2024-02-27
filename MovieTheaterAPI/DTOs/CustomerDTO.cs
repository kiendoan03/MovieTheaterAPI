using MovieTheaterAPI.Entities;
using System.ComponentModel.DataAnnotations;

namespace MovieTheaterAPI.DTOs
{
    public class CustomerDTO : UserDTO
    {
        public ICollection<TicketDTO>? Tickets { get;  } = new List<TicketDTO>();
    }
}
