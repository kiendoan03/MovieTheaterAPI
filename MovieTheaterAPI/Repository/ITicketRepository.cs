using MovieTheaterAPI.Entities;

namespace MovieTheaterAPI.Repository
{
    public interface ITicketRepository : IRepository<Ticket>
    {
        Task<List<Ticket>> GetTicketToBooking();
        Task<IEnumerable<Ticket>> GetTicketsBySchedule(int scheduleId);
        Task<IEnumerable<Ticket>> GetTicketsByCustomer(int customerId);
        Task<IEnumerable<Ticket>> GetTicketsOrdering();
    }
}
