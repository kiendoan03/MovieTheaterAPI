using MovieTheaterAPI.Entities;
using System.ComponentModel.DataAnnotations;

namespace MovieTheaterAPI.DTOs
{
    public class StaffDTO
    {
        public int Id { get; set; }
        public string StaffName { get; set; } = null!;
        public string StaffImage { get; set; } = null!;
        public string StaffRole { get; set; } = null!;
        [DataType(DataType.EmailAddress)]
        public string StaffEmail { get; set; } = null!;
        [MinLength(10), MaxLength(10)]
        public string StaffPhone { get; set; } = null!;
        public string StaffUsername { get; set; } = null!;
        [MinLength(8)]
        public string StaffPassword { get; set; } = null!;
        [DataType(DataType.Date)]
        public string? StaffDOB { get; set; }
        public string StaffAddress { get; set; } = null!;
        public string StaffImg { get; set; } = null!;
        public ICollection<TicketDTO>? Tickets { get;  } = new List<TicketDTO>();
    }
}
