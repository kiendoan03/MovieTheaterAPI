using Microsoft.EntityFrameworkCore;
using MovieTheaterAPI.DAL;
using MovieTheaterAPI.Entities;

namespace MovieTheaterAPI.Repository
{
    public class TicketRepository : Repository<Ticket>, ITicketRepository
    {
        public TicketRepository(MovieTheaterDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Ticket>> GetTicketsByCustomer(int customerId)
        {
            return await _context.Tickets.Where(x => x.CustomerId == customerId).ToListAsync();
        }

        public async Task<IEnumerable<Ticket>> GetTicketsBySchedule(int scheduleId)
        {
            return await _context.Tickets.Where(x => x.ScheduleId == scheduleId).ToListAsync();
        }

        public async Task<List<Ticket>> GetTicketToBooking()
        {
            return await _context.Tickets.Where(x => x.status == 1).ToListAsync();
        }
    }
}

