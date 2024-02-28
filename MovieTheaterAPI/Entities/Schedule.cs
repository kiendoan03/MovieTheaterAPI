namespace MovieTheaterAPI.Entities
{
    public class Schedule
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; } = null!;
        public int RoomId { get; set; }
        public Room Room { get; set; } = null!;
        public DateOnly ScheduleDate { get; set; }
        public TimeOnly? StartTime { get; set; }
        public TimeOnly? EndTime { get; set; }
        public List<Seat> Seats { get; set; } = [];
        public List<Ticket> Tickets { get; set; } = [];
    }
}
