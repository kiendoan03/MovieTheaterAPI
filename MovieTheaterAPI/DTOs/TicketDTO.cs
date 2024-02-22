﻿using MovieTheaterAPI.Entities;

namespace MovieTheaterAPI.DTOs
{
    public class TicketDTO
    {
        public int Id { get; set; }
        public int SeatId { get; set; } 
        public int ScheduleId { get; set; }
        public int FinalPrice { get; set; }
        //public SeatDTO Seats { get; set; } = null!;
        //public ScheduleDTO Schedules { get; set; } = null!;
        public int? CustomerId { get; set; }
        //public CustomerDTO? Customer { get; set; }
        public int? StaffId { get; set; }
        //public StaffDTO? Staff { get; set; }
        public int status { get; set; }
    }
}
