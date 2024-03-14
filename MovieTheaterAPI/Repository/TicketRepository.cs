using Microsoft.AspNetCore.Mvc;
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

        public async Task<IEnumerable<Ticket>> GetTicketsByCustomer([FromServices] IHttpContextAccessor httpContextAccessor)
        {
            var userIdClaim = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId");
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return await _context.Tickets.Where(x => x.CustomerId == userId).Where(y => y.status == 2).ToListAsync();
            }
            else
            {
                throw new UnauthorizedAccessException("Invalid user");
            }
        }

        public async Task<IEnumerable<Ticket>> GetTicketsBySchedule(int scheduleId)
        {
            return await _context.Tickets
                .Include(x => x.Seats)
                .Where(x => x.ScheduleId == scheduleId).ToListAsync();
        }
        public async Task<IEnumerable<Ticket>> GetTicketsByScheduleToDelete(int scheduleId)
        {
            return await _context.Tickets
                .Where(x => x.ScheduleId == scheduleId).ToListAsync();
        }

        public async Task<IEnumerable<Ticket>> GetTicketsOrdering([FromServices] IHttpContextAccessor httpContextAccessor)
        {
            var userIdClaim = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId");
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return await _context.Tickets
                    .Include(x => x.Seats)
                    .Where(x => x.status == 1).Where(y => y.CustomerId == userId)
                    .ToListAsync();
            }
            else
            {
                throw new UnauthorizedAccessException("Invalid user");
            }

                
        }

        public async Task<List<Ticket>> GetTicketToBooking([FromServices] IHttpContextAccessor httpContextAccessor)
        {
            var userIdClaim = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId");
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return await _context.Tickets.Where(x => x.status == 1).Where(y => y.CustomerId == userId).ToListAsync();
            }
            else
            {
                throw new UnauthorizedAccessException("Invalid user");
            }
        }
    }
}

