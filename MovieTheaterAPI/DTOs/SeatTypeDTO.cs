using MovieTheaterAPI.Entities;

namespace MovieTheaterAPI.DTOs
{
    public class SeatTypeDTO
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public int Price { get; set; }
        public ICollection<SeatDTO> Seats { get; set; } = new List<SeatDTO>();
    }
}
