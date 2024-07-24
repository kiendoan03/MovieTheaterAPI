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

        public async Task<bool> CheckDuplicateCustomer(string username, string email)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(x => x.UserName == username || x.Email == email);
            if (customer == null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> CheckDuplicateCustomerExcept(string username, string email, int id)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(x => (x.UserName == username || x.Email == email) && x.Id != id);
            if (customer == null)
            {
                return false;
            }
            return true;
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
