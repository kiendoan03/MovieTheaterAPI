using AutoMapper;
using MovieTheaterAPI.DTOs;
using MovieTheaterAPI.Entities;
using System.Globalization;

namespace MovieTheaterAPI.AutoMapper
{
    public class AppMapperProfile : Profile
    {
        public AppMapperProfile()
        {
            CreateMap<string, TimeOnly>().ConvertUsing(src => TimeOnly.Parse(src));
            CreateMap<string, DateOnly>().ConvertUsing(src => DateOnly.Parse(src, CultureInfo.InvariantCulture));
            CreateMap<Cast, CastDTO>().ReverseMap();
            CreateMap<Director, DirectorDTO>().ReverseMap();
            CreateMap<SeatType, SeatTypeDTO>().ReverseMap();
            CreateMap<Genre, GenreDTO>().ReverseMap();
            CreateMap<Movie, MovieDTO>().ReverseMap();
            CreateMap<Room, RoomDTO>().ReverseMap();
            CreateMap<Seat , SeatDTO>().ReverseMap();
            CreateMap<Schedule, ScheduleDTO>().ReverseMap();
            CreateMap<Ticket, TicketDTO>().ReverseMap();
            CreateMap<MovieCast, MovieCastDTO>().ReverseMap();
            CreateMap<MovieDirector, MovieDirectorDTO>().ReverseMap();
            CreateMap<MovieGenre, MovieGenreDTO>().ReverseMap();
            CreateMap<Customer, CustomerDTO>().ReverseMap();
            CreateMap<Staff, StaffDTO>().ReverseMap();
            CreateMap<RoomType, RoomTypeDTO>().ReverseMap();
        }
    }
}
