using MovieTheaterAPI.DAL;
using MovieTheaterAPI.Entities;
using MovieTheaterAPI.Repository.Interfaces;

namespace MovieTheaterAPI.Repository
{
    public class RoomRepository : Repository<Room> , IRoomRepository
    {
        public RoomRepository(MovieTheaterDbContext context) : base(context)
        {
        }

        public async Task<bool> CheckRoomExistInTicket(int roomId)
        {
            var tickets = from t in _context.Tickets
                          where t.Schedules.RoomId == roomId
                          select t;
            return await Task.FromResult(tickets.Any());
        }
    }
}
