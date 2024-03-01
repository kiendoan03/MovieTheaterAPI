using Microsoft.EntityFrameworkCore;
using MovieTheaterAPI.DAL;
using MovieTheaterAPI.Entities;
using MovieTheaterAPI.Repository.Interfaces;

namespace MovieTheaterAPI.Repository
{
    public class SeatRepository : Repository<Seat> , ISeatRepository
    {
        public SeatRepository(MovieTheaterDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Seat>> GetSeatsByRoomId(int roomId)
        {
            return await _context.Seats
                .Include(s => s.SeatType)
                .Where(s => s.RoomId == roomId)
                .ToListAsync();
        }
    }
}
