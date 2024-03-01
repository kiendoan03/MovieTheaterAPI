using MovieTheaterAPI.Entities;

namespace MovieTheaterAPI.Services.Interfaces
{
    public interface ITicketService
    {
        Task<IEnumerable<Ticket>> GetTicketsByCustomer(int customerId);
        Task<IEnumerable<Ticket>> GetTicketsBySchedule(int scheduleId);
        Task OrderTicket(int id);
        Task<IEnumerable<Ticket>> GetTicketsOrdering();
        Task BookingTickets();
    }
}
