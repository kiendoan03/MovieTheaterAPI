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
    }
}
