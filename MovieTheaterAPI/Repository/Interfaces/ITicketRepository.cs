using Microsoft.AspNetCore.Mvc;
using MovieTheaterAPI.Entities;

namespace MovieTheaterAPI.Repository.Interfaces
{
    public interface ITicketRepository : IRepository<Ticket>
    {
        Task<List<Ticket>> GetTicketToBooking(int cusId);
        Task<IEnumerable<Ticket>> GetTicketsBySchedule(int scheduleId);
        Task<IEnumerable<Ticket>> GetTicketsByScheduleToDelete(int scheduleId);
        Task<IEnumerable<Ticket>> GetTicketsByCustomer(int cusId);
        Task<IEnumerable<Ticket>> GetTicketsOrdering(int cusId);
        Task<IEnumerable<Ticket>> CheckTicket(int scheduleId);
    }
}
