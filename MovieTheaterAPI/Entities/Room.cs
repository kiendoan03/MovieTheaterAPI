namespace MovieTheaterAPI.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public string RoomName { get; set; } = null!;
        public int RoomTypeId { get; set; }
        public RoomType RoomType { get; set; } = null!;
        public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
        public ICollection<Seat> Seats { get;  } = new List<Seat>();
    }
}
