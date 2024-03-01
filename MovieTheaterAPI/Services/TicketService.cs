using AutoMapper;
using MovieTheaterAPI.Entities;
using MovieTheaterAPI.Repository;
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

        public async Task BookingTickets()
        {
            var bookTickets = await _unitOfWork.TicketRepository.GetTicketToBooking();
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

        public async Task<IEnumerable<Ticket>> GetTicketsByCustomer(int customerId)
        {
            var tickets = await _unitOfWork.TicketRepository.GetTicketsByCustomer(customerId);
            return tickets;
        }

        public async Task<IEnumerable<Ticket>> GetTicketsBySchedule(int scheduleId)
        {
            var tickets = await _unitOfWork.TicketRepository.GetTicketsBySchedule(scheduleId);
            return tickets;
        }

        public async Task<IEnumerable<Ticket>> GetTicketsOrdering()
        {
            var tickets = await _unitOfWork.TicketRepository.GetTicketsOrdering();
            return tickets;
        }

        public async Task OrderTicket(int id)
        {
            var existingTicket = await _unitOfWork.TicketRepository.GetById(id);
            var customerId = 1;
            if (existingTicket == null)
            {
                throw new ArgumentException("Ticket not found.");
            }

            existingTicket.status = existingTicket.status == 0 ? 1 : (existingTicket.status == 1 ? 0 : throw new ArgumentException("Seat has already been reserved"));
            existingTicket.CustomerId = existingTicket.status == 0 ? null : customerId;

            await _unitOfWork.TicketRepository.Update(existingTicket);
            await _unitOfWork.Save();
        }
    }
}
