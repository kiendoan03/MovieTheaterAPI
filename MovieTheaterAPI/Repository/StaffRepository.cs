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

        public async Task<bool> CheckDuplicateStaff(string username, string email)
        {
            var staff = await _context.Staffs.FirstOrDefaultAsync(x => x.UserName == username || x.Email == email);
            if (staff == null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> CheckDuplicateStaffExcept(string username, string email, int id)
        {
            var staff = await _context.Staffs.FirstOrDefaultAsync(x => (x.UserName == username || x.Email == email) && x.Id != id);
            if (staff == null)
            {
                return false;
            }
            return true;
        }
        //public async Task<Staff> Login(string username, string password)
        //{
        //    return await _context.Staffs.FirstOrDefaultAsync(x => x.Username == username && x.Password == password);
        //}
    }
}
