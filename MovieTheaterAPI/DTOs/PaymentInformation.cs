using MovieTheaterAPI.Entities;

namespace MovieTheaterAPI.DTOs
{
    public class PaymentInformation
    {
        public List<Ticket> Tickets { get; set; }
        public int TotalPrice { get; set; }
        public int ScheduleId { get; set; }
    }
}
