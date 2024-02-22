using MovieTheaterAPI.Entities;

namespace MovieTheaterAPI.DTOs
{
    public class RoomDTO
    {
        public int Id { get; set; }
        public string RoomName { get; set; } = null!;
        //public int RoomCapacity { get; } = 66;
        public int RoomTypeId { get; set; }
        public ICollection<ScheduleDTO>? Schedules { get; } = new List<ScheduleDTO>();
        public ICollection<SeatDTO>? Seats { get;  } = new List<SeatDTO>();
    }
}
