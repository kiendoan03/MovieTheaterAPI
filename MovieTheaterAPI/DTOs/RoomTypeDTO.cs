using MovieTheaterAPI.Entities;

namespace MovieTheaterAPI.DTOs
{
    public class RoomTypeDTO
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int Capacity { get; set; }
        public ICollection<RoomDTO> Rooms { get; set; } = new List<RoomDTO>();
    }
}

