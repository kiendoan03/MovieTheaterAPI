using Microsoft.EntityFrameworkCore;
using MovieTheaterAPI.DAL;
using MovieTheaterAPI.Entities;
using MovieTheaterAPI.Repository.Interfaces;

namespace MovieTheaterAPI.Repository
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(MovieTheaterDbContext context) : base(context)
        {
        }

        public async Task<int> CountTicketsBought(int id)
        {
            var ticket = await _context.Tickets.Where(x => x.CustomerId == id).Where(y => y.status == 2).ToListAsync();
            return ticket.Count;
        }
        //public async Task<Customer> Login(string Username, string password)
        //{
        //    return await _context.Customers.FirstOrDefaultAsync(x => x.Username == Username && x.Password == password);
        //}
    }
}
