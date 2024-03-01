using MovieTheaterAPI.Entities;

namespace MovieTheaterAPI.Repository.Interfaces
{
    public interface IScheduleRepository : IRepository<Schedule>
    {
        Task<IEnumerable<Schedule>> GetSchedulesByMovie(int movieId);
        Task<IEnumerable<Schedule>> GetSchedulesWithMovieRoom();
        Task<IEnumerable<Schedule>> GetSchedulesByDateAndRoom(int roomId, DateOnly scheduleDate);
        Task<IEnumerable<Schedule>> GetSchedulesByDateAndRoomExceptOne(int roomId, DateOnly scheduleDate, int exceptScheduleId);
    }
}
