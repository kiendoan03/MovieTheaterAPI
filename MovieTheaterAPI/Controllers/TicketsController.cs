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
        //private readonly MovieTheaterDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TicketsController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            //_context = context;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("get-tickets-by-customer")]
        public async Task<ActionResult<IEnumerable<TicketDTO>>> GetTicketsByCustomer(int customerId)
        {
            //var tickets = await _context.Tickets.ToListAsync();
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
            //var tickets = await _context.Tickets.ToListAsync();
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
            //if (id != ticket.Id)
            //{
            //    return BadRequest();
            //}

            var existingTicket = await _unitOfWork.TicketRepository.GetById(id);

            if (existingTicket == null)
            {
                return NotFound();
            }

            /*//await _unitOfWork.TicketRepository.Detached(existingTicket);

            //var updatedTicket = _mapper.Map<Ticket>(ticket);
            //updatedTicket.ScheduleId = existingTicket.ScheduleId;
            //updatedTicket.SeatId = existingTicket.SeatId;
            //updatedTicket.FinalPrice = existingTicket.FinalPrice;
            //updatedTicket.CustomerId = existingTicket.CustomerId;
            //updatedTicket.StaffId = existingTicket.StaffId;

            //updatedTicket.status = existingTicket.status == 0 ? 1 : 0;
            //_ = existingTicket.status == 0 ? 1 : (existingTicket.status == 1 ? 0 : throw new ArgumentException("Seat has already been reserved"));*/

            existingTicket.status = existingTicket.status == 0 ? 1 : (existingTicket.status == 1 ? 0 : throw new ArgumentException("Seat has already been reserved"));


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
            /*if (ticketIds == null || ticketIds.Count == 0)
            {
                return BadRequest("No tickets provided for update.");
            }

            foreach (var ticket in ticketIds)
            {
                var existingTicket = await _unitOfWork.TicketRepository.GetTicketToBooking(ticket);

                if (existingTicket == null)
                {
                    return NotFound($"Ticket with id {ticket} not found.");
                }

                await _unitOfWork.TicketRepository.Detached(existingTicket);

                var updatedTicket = _mapper.Map<Ticket>(ticket);

                // Tiến hành cập nhật cho mỗi bản ghi
                updatedTicket.status = 3; // Thay đổi các thuộc tính cần cập nhật
                updatedTicket.SeatId = existingTicket.SeatId;
                updatedTicket.ScheduleId = existingTicket.ScheduleId;
                updatedTicket.FinalPrice = existingTicket.FinalPrice;

                await _unitOfWork.TicketRepository.Update(updatedTicket);
            }*/

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
