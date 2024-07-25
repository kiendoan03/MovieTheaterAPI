using MovieTheaterAPI.Entities;

namespace MovieTheaterAPI.Repository.Interfaces
{
    public interface IRoomRepository : IRepository<Room>
    {
        //CheckRoomExistInTicket
        Task<bool> CheckRoomExistInTicket(int roomId);
    }
}
