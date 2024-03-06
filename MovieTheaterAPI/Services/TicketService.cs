using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieTheaterAPI.Entities;
using MovieTheaterAPI.Repository.Interfaces;
using MovieTheaterAPI.Services.Interfaces;

namespace MovieTheaterAPI.Services
{
    public class TicketService : ITicketService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TicketService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task BookingTickets([FromServices] IHttpContextAccessor httpContextAccessor)
        {
            var bookTickets = await _unitOfWork.TicketRepository.GetTicketToBooking(httpContextAccessor);
            if (bookTickets == null || !bookTickets.Any())
            {
                throw new InvalidOperationException("No tickets found for booking.");
            }

            foreach (var ticket in bookTickets)
            {
                ticket.status = 2;
                await _unitOfWork.TicketRepository.Update(ticket);
            }

            await _unitOfWork.Save();
        }

        public async Task<IEnumerable<Ticket>> GetTicketsByCustomer([FromServices] IHttpContextAccessor httpContextAccessor)
        {
            var tickets = await _unitOfWork.TicketRepository.GetTicketsByCustomer(httpContextAccessor);
            return tickets;
        }

        public async Task<IEnumerable<Ticket>> GetTicketsBySchedule(int scheduleId)
        {
            var tickets = await _unitOfWork.TicketRepository.GetTicketsBySchedule(scheduleId);
            return tickets;
        }

        public async Task<IEnumerable<Ticket>> GetTicketsOrdering([FromServices] IHttpContextAccessor httpContextAccessor)
        {
            var tickets = await _unitOfWork.TicketRepository.GetTicketsOrdering(httpContextAccessor);
            return tickets;
        }

        public async Task OrderTicket(int id, [FromServices] IHttpContextAccessor httpContextAccessor)
        {
            var existingTicket = await _unitOfWork.TicketRepository.GetById(id);
            var userIdClaim = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId");
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                var customerId = userId;
                if (existingTicket == null)
                {
                    throw new InvalidOperationException("Ticket not found.");
                }

                existingTicket.status = existingTicket.status == 0 ? 1 : (existingTicket.status == 1 ? 0 : throw new InvalidOperationException("Seat has already been reserved"));
                existingTicket.CustomerId = existingTicket.status == 0 ? null : customerId;

                await _unitOfWork.TicketRepository.Update(existingTicket);
                await _unitOfWork.Save();
            }
        }
    }
}
