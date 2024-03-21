using Microsoft.AspNetCore.Mvc;
using MovieTheaterAPI.Entities;

namespace MovieTheaterAPI.Services.Interfaces
{
    public interface ITicketService
    {
        Task<IEnumerable<Ticket>> GetTicketsByCustomer(int cusId);
        Task<IEnumerable<Ticket>> GetTicketsBySchedule(int scheduleId);
        Task OrderTicket(int id, int cusId);
        Task<IEnumerable<Ticket>> GetTicketsOrdering(int cusId);
        Task BookingTickets(int cusId);
    }
}
