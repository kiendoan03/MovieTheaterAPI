using MovieTheaterAPI.Entities;

namespace MovieTheaterAPI.Repository.Interfaces
{
    public interface ISeatRepository : IRepository<Seat>
    {
        Task<IEnumerable<Seat>> GetSeatsByRoomId(int roomId);
    }
}
