namespace MovieTheaterAPI.Entities
{
    public class RoomType
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int Capacity { get; set; }
        public ICollection<Room> Rooms { get; set; } = new List<Room>();
    }
}
