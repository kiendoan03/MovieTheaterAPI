using MovieTheaterAPI.Entities;

namespace MovieTheaterAPI.Repository
{
    public interface IScheduleRepository : IRepository<Schedule>
    {
        Task<IEnumerable<Schedule>> GetSchedulesByMovie(int movieId);
        Task<IEnumerable<Schedule>> GetSchedulesWithMovieRoom();
        Task<IEnumerable<Schedule>> GetSchedulesByDateAndRoom(int roomId, DateOnly scheduleDate);
    }
}
