using MovieTheaterAPI.Entities;

namespace MovieTheaterAPI.DTOs
{
    public class SeatDTO
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        //public bool IsReserved { get; set; }
        //public RoomDTO Room { get; set; } = null!;
        public int SeatTypeId { get; set; }
        //public SeatTypeDTO SeatType { get; set; } = null!;
        public List<ScheduleDTO> Schedules { get;  } = [];
        public List<TicketDTO> Tickets { get; } = [];
    }
}
