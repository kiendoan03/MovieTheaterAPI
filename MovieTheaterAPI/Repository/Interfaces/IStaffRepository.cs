using MovieTheaterAPI.Entities;

namespace MovieTheaterAPI.Repository.Interfaces
{
    public interface IStaffRepository : IRepository<Staff>
    {
        //Task<Staff> Login(string username, string password);
        Task<bool> CheckDuplicateStaff(string username, string email);
        //CheckDuplicateStaffExcept
        Task<bool> CheckDuplicateStaffExcept(string username, string email, int id);
    }
}
