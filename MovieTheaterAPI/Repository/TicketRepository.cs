using Microsoft.EntityFrameworkCore;
using MovieTheaterAPI.DAL;
using MovieTheaterAPI.Entities;
using MovieTheaterAPI.Repository.Interfaces;

namespace MovieTheaterAPI.Repository
{
    public class TicketRepository : Repository<Ticket>, ITicketRepository
    {
        public TicketRepository(MovieTheaterDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Ticket>> CheckTicket(int scheduleId)
        {
            var tickets = await _context.Tickets
                .Where(x => x.ScheduleId == scheduleId)
                .Where(y => y.status != 0)
                .ToListAsync();
            return tickets;
        }

        public async Task<IEnumerable<Ticket>> GetTicketsByCustomer(int customerId)
        {
            return await _context.Tickets.Where(x => x.CustomerId == customerId).Where(y => y.status == 2).ToListAsync();
        }

        public async Task<IEnumerable<Ticket>> GetTicketsBySchedule(int scheduleId)
        {
            return await _context.Tickets.Where(x => x.ScheduleId == scheduleId).ToListAsync();
        }

        public async Task<IEnumerable<Ticket>> GetTicketsOrdering()
        {
            return await _context.Tickets
                .Include(x => x.Seats)
                .Where(x => x.status == 1).Where(y => y.CustomerId == 1)
                .ToListAsync();
        }

        public async Task<List<Ticket>> GetTicketToBooking()
        {
            return await _context.Tickets.Where(x => x.status == 1).Where(y => y.CustomerId == 1).ToListAsync();
        }
    }
}

