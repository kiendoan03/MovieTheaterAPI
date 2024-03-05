using Microsoft.AspNetCore.Mvc;
using MovieTheaterAPI.Entities;

namespace MovieTheaterAPI.Services.Interfaces
{
    public interface ITicketService
    {
        Task<IEnumerable<Ticket>> GetTicketsByCustomer([FromServices] IHttpContextAccessor httpContextAccessor);
        Task<IEnumerable<Ticket>> GetTicketsBySchedule(int scheduleId);
        Task OrderTicket(int id, [FromServices] IHttpContextAccessor httpContextAccessor);
        Task<IEnumerable<Ticket>> GetTicketsOrdering([FromServices] IHttpContextAccessor httpContextAccessor);
        Task BookingTickets([FromServices] IHttpContextAccessor httpContextAccessor);
    }
}
