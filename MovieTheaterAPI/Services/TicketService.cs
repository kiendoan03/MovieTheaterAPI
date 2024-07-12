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

        public async Task BookingTickets(int cusId)
        {
            var bookTickets = await _unitOfWork.TicketRepository.GetTicketToBooking(cusId);
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

        public async Task<int> CountTicketsSold()
        {
            var tickets = await _unitOfWork.TicketRepository.GetTicketSold();
            return tickets.Count();
        }

        public async Task<IEnumerable<Ticket>> GetTicketsByCustomer(int cusId)
        {
            var tickets = await _unitOfWork.TicketRepository.GetTicketsByCustomer(cusId);
            return tickets;
        }

        public async Task<IEnumerable<Ticket>> GetTicketsBySchedule(int scheduleId)
        {
            var tickets = await _unitOfWork.TicketRepository.GetTicketsBySchedule(scheduleId);
            return tickets;
        }

        public async Task<IEnumerable<Ticket>> GetTicketsOrdering(int cusId)
        {
            var tickets = await _unitOfWork.TicketRepository.GetTicketsOrdering(cusId);
            return tickets;
        }

        public async Task<int> GetTotalIncome()
        {
            var tickets = await _unitOfWork.TicketRepository.GetTicketSold();
            return (int)tickets.Sum(x => x.FinalPrice);
        }

        public async Task OrderTicket(int id, int cusId)
        {
            var existingTicket = await _unitOfWork.TicketRepository.GetById(id);
            //var userIdClaim = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId");
            //if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            //{
                var customerId = cusId;
                if (existingTicket == null)
                {
                    throw new InvalidOperationException("Ticket not found.");
                }

                existingTicket.status = existingTicket.status == 0 ? 1 : (existingTicket.status == 1 ? 0 : throw new InvalidOperationException("Seat has already been reserved"));
                existingTicket.CustomerId = existingTicket.status == 0 ? null : customerId;

                await _unitOfWork.TicketRepository.Update(existingTicket);
                await _unitOfWork.Save();
            //}
        }
    }
}
