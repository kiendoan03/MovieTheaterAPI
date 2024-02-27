using Microsoft.EntityFrameworkCore;
using MovieTheaterAPI.DAL;
using MovieTheaterAPI.Entities;

namespace MovieTheaterAPI.Repository
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(MovieTheaterDbContext context) : base(context)
        {
        }
        public async Task<Customer> Login(string Username, string password)
        {
            return await _context.Customers.FirstOrDefaultAsync(x => x.Username == Username && x.Password == password);
        }
    }
}
