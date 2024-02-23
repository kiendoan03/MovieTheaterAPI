namespace MovieTheaterAPI.Entities
{
    public class Seat
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public Room Room { get; set; } = null!;
        public int SeatTypeId { get; set; }
        public SeatType SeatType { get; set; } = null!;
        public List<Schedule> Schedules { get; } = [];
        public List<Ticket> Tickets { get; } = [];
    }
}
