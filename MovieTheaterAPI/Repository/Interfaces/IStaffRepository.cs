using MovieTheaterAPI.Entities;

namespace MovieTheaterAPI.Repository.Interfaces
{
    public interface IStaffRepository : IRepository<Staff>
    {
        Task<Staff> Login(string username, string password);
    }
}
