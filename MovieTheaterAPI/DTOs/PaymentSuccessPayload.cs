using MovieTheaterAPI.Entities;

namespace MovieTheaterAPI.DTOs
{
    public class PaymentSuccessPayload
    {
        public string CustomerEmail { get; set; }
        public int TotalPrice { get; set; }
        public List<Ticket>? Tickets { get; set; }
        public ScheduleDTO? Schedule { get; set; }
    }
}
