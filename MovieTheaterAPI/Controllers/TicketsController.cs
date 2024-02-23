using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTheaterAPI.DAL;
using MovieTheaterAPI.DTOs;
using MovieTheaterAPI.Entities;
using MovieTheaterAPI.Repository;

namespace MovieTheaterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TicketsController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("get-tickets-by-customer")]
        public async Task<ActionResult<IEnumerable<TicketDTO>>> GetTicketsByCustomer(int customerId)
        {
            var tickets = await _unitOfWork.TicketRepository.GetTicketsByCustomer(customerId);
            if(tickets == null)
            {
                   return NotFound();
            }
            var ticketDTOs = _mapper.Map<List<TicketDTO>>(tickets);
           
            return ticketDTOs;
        }

        [HttpGet]
        [Route("get-tickets-by-schedule")]
        public async Task<ActionResult<IEnumerable<TicketDTO>>> GetTickets(int scheduleId)
        {
            var tickets = await _unitOfWork.TicketRepository.GetTicketsBySchedule(scheduleId);
            if(tickets == null)
            {
                   return NotFound();
            }
            var ticketDTOs = _mapper.Map<List<TicketDTO>>(tickets);
           
            return ticketDTOs;
        }


        [HttpPut ("{id}")]
        public async Task<IActionResult> OrderTicket(int id)
        {
            var existingTicket = await _unitOfWork.TicketRepository.GetById(id);

            if (existingTicket == null)
            {
                return NotFound();
            }

            existingTicket.status = existingTicket.status == 0 ? 1 : (existingTicket.status == 1 ? 0 : throw new ArgumentException("Seat has already been reserved"));
            existingTicket.CustomerId = 1;

            await _unitOfWork.TicketRepository.Update(existingTicket);

            try
            {
                await _unitOfWork.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (TicketExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPut("update-multiple")]
        public async Task<IActionResult> BookingTickets()
        {
            var bookTickets = await _unitOfWork.TicketRepository.GetTicketToBooking();
            if (bookTickets == null)
            {
                   return NotFound("No tickets found for booking.");
            }
            foreach (var ticket in bookTickets)
            {
                ticket.status = 2;
                await _unitOfWork.TicketRepository.Update(ticket);
            }

            try
            {
                await _unitOfWork.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Error occurred while booking tickets. Please try again later.");
            }

            return NoContent();
        }

        private bool TicketExists(int id)
        {
            return _unitOfWork.TicketRepository.IsExists(id).Result;
        }
    }
}
