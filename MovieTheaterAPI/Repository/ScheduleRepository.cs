using Microsoft.EntityFrameworkCore;
using MovieTheaterAPI.DAL;
using MovieTheaterAPI.Entities;

namespace MovieTheaterAPI.Repository
{
    public class ScheduleRepository : Repository<Schedule>, IScheduleRepository
    {
        public ScheduleRepository(MovieTheaterDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Schedule>> GetSchedulesByDateAndRoom(int roomId, DateOnly scheduleDate)
        {
            return await _context.Schedules
                .Where(s => s.RoomId == roomId && s.ScheduleDate == scheduleDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Schedule>> GetSchedulesByMovie(int movieId)
        {
            var schedules = from s in _context.Schedules
                            join t in _context.Rooms on s.RoomId equals t.Id
                            where s.MovieId == movieId
                            select new Schedule
                            {
                                Id = s.Id,
                                Room = t,
                                ScheduleDate = s.ScheduleDate,
                                StartTime = s.StartTime,
                                EndTime = s.EndTime,
                                // Add other properties from the Schedule entity as needed
                            };
            return await Task.FromResult(schedules);
        }

        public async Task<IEnumerable<Schedule>> GetSchedulesDetails()
        {
            return await _context.Schedules
                .Include(s => s.Movie)
                .Include(s => s.Room)
                .ToListAsync();
        }
    }
}
