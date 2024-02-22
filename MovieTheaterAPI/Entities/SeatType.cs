namespace MovieTheaterAPI.Entities
{
    public class SeatType
    {
        public int Id { get; set; }
        public int Type { get; set; } 
        public int Price { get; set; }
        public ICollection<Seat> Seats { get; set; } = new List<Seat>();
    }
}
