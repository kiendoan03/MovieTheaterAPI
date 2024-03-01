using Microsoft.EntityFrameworkCore;
using MovieTheaterAPI.DAL;
using MovieTheaterAPI.Entities;
using MovieTheaterAPI.Repository.Interfaces;

namespace MovieTheaterAPI.Repository
{
    public class StaffRepository : Repository<Staff>, IStaffRepository
    {
        public StaffRepository(MovieTheaterDbContext context) : base(context)
        {
        }
        public async Task<Staff> Login(string username, string password)
        {
            return await _context.Staffs.FirstOrDefaultAsync(x => x.Username == username && x.Password == password);
        }
    }
}
