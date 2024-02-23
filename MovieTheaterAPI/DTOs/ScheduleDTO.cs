using MovieTheaterAPI.Entities;
using System.ComponentModel.DataAnnotations;

namespace MovieTheaterAPI.DTOs
{
    public class ScheduleDTO
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public int RoomId { get; set; }
        [DataType(DataType.Date)]
        public string? ScheduleDate { get; set; }
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public string? StartTime { get; set; }
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public string? EndTime { get; set; }
        public List<SeatDTO> Seats { get;  } = [];
        public List<TicketDTO> Tickets { get;} = [];
    }
}
