using MovieTheaterAPI.DAL;
using MovieTheaterAPI.Entities;

namespace MovieTheaterAPI.Repository
{
    public class RoomRepository : Repository<Room> , IRoomRepository
    {
        public RoomRepository(MovieTheaterDbContext context) : base(context)
        {
        }
    }
}
