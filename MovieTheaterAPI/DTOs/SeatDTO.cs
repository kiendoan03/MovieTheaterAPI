using MovieTheaterAPI.Entities;

namespace MovieTheaterAPI.DTOs
{
    public class SeatDTO
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public int SeatTypeId { get; set; }
        public List<ScheduleDTO> Schedules { get;  } = [];
        public List<TicketDTO> Tickets { get; } = [];
    }
}
