using MovieTheaterAPI.DTOs;
using MovieTheaterAPI.Entities;

namespace MovieTheaterAPI.Services.Interfaces
{
    public interface IScheduleService
    {
        Task<IEnumerable<ScheduleDTO>> GetAllSchedules();
        Task<ScheduleDTO> GetScheduleById(int id);
        Task UpdateSchedule(int id, ScheduleDTO scheduleDTO);
        Task<ScheduleDTO> CreateSchedule(ScheduleDTO scheduleDTO);
        Task DeleteSchedule(int id);
        Task<IEnumerable<ScheduleDTO>> GetSchedulesByMovie(int movieId);
    }
}
