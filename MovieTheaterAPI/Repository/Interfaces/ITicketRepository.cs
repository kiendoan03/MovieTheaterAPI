using Microsoft.AspNetCore.Mvc;
using MovieTheaterAPI.Entities;

namespace MovieTheaterAPI.Repository.Interfaces
{
    public interface ITicketRepository : IRepository<Ticket>
    {
        Task<List<Ticket>> GetTicketToBooking([FromServices] IHttpContextAccessor httpContextAccessor);
        Task<IEnumerable<Ticket>> GetTicketsBySchedule(int scheduleId);
        Task<IEnumerable<Ticket>> GetTicketsByCustomer([FromServices] IHttpContextAccessor httpContextAccessor);
        Task<IEnumerable<Ticket>> GetTicketsOrdering([FromServices] IHttpContextAccessor httpContextAccessor);
        Task<IEnumerable<Ticket>> CheckTicket(int scheduleId);
    }
}
