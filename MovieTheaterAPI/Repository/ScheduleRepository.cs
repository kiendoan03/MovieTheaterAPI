using Microsoft.EntityFrameworkCore;
using MovieTheaterAPI.DAL;
using MovieTheaterAPI.Entities;
using MovieTheaterAPI.Repository.Interfaces;

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

        public async Task<IEnumerable<Schedule>> GetSchedulesByDateAndRoomExceptOne(int roomId, DateOnly scheduleDate, int exceptScheduleId)
        {
            var schedules = await _context.Schedules
                .Where(s => s.RoomId == roomId && s.ScheduleDate == scheduleDate && s.Id != exceptScheduleId)
                .ToListAsync();

            return schedules;
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

        public async Task<IEnumerable<Schedule>> GetSchedulesWithMovieRoom()
        {
            var schedule = from s in _context.Schedules
                           join m in _context.Movies on s.MovieId equals m.Id
                           join r in _context.Rooms on s.RoomId equals r.Id
                           select new Schedule
                           {
                               Id = s.Id,
                               Movie = m,
                               Room = r,
                               ScheduleDate = s.ScheduleDate,
                               StartTime = s.StartTime,
                               EndTime = s.EndTime,
                               // Add other properties from the Schedule entity as needed
                           };
            return await Task.FromResult(schedule);
        }

        public async Task<Schedule> GetScheduleWithDetail(int scheduleId)
        {
            var schedule = await _context.Schedules
                .Include(s => s.Movie)
                .Include(s => s.Room)
                .FirstOrDefaultAsync(s => s.Id == scheduleId);
            return schedule;

            //var schedule = from s in _context.Schedules
            //               join m in _context.Movies on s.MovieId equals m.Id
            //               join r in _context.Rooms on s.RoomId equals r.Id
            //               where s.Id == scheduleId
            //               select new Schedule
            //               {
            //                   Id = s.Id,
            //                   RoomId = r.Id,
            //                   Movie = m,
            //                   Room = r,
            //                   ScheduleDate = s.ScheduleDate,
            //                   StartTime = s.StartTime,
            //                   EndTime = s.EndTime,
            //                   // Add other properties from the Schedule entity as needed
            //               };
            //return await Task.FromResult(schedule.FirstOrDefault());
        }
    }
}
