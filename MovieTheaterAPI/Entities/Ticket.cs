using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Numerics;

namespace MovieTheaterAPI.Entities
{
    public class Ticket
    {
        public int Id { get; set; }
        public int SeatId { get; set; }
        public int ScheduleId { get; set; }
        public int? FinalPrice { get; set; }
        public Seat Seats { get; set; } = null!;
        public Schedule Schedules { get; set; } = null!;
        public int? CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public int? StaffId { get; set; } 
        public Staff? Staff { get; set; } 
        public int status { get; set; }
    }
}
