using MovieTheaterAPI.Entities;

namespace MovieTheaterAPI.Repository
{
    public interface ISeatRepository : IRepository<Seat>
    { 
        Task<IEnumerable<Seat>> GetSeatsByRoomId(int roomId);
    }
}
