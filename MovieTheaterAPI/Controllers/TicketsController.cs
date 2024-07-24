using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MovieTheaterAPI.DAL;
using MovieTheaterAPI.DTOs;
using MovieTheaterAPI.Entities;
using MovieTheaterAPI.Hubs;
using MovieTheaterAPI.Repository;
using MovieTheaterAPI.Services.Interfaces;

namespace MovieTheaterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize (Roles = "Customer")]

    public class TicketsController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        private readonly IHubContext<BookticketHub> _hubContext;

        public TicketsController(ITicketService ticketService, IHubContext<BookticketHub> hubContext)
        {
            _ticketService = ticketService;
            _hubContext = hubContext;
        }

        [HttpGet]
        [Authorize (Roles = "Manager")]
        [Route("count-tickets-sold")]
        public async Task<ActionResult<int>> CountTicketsSold()
        {
            var tickets = await _ticketService.CountTicketsSold();
            return Ok(tickets);
        }

        [HttpGet]
        [Route("get-tickets-by-customer")]
        //[Authorize(Roles = "Customer")]
        public async Task<ActionResult<IEnumerable<TicketDTO>>> GetTicketsByCustomer(int cusId)
        {
            var tickets = await _ticketService.GetTicketsByCustomer(cusId);
            if (tickets == null)
            {
                return NotFound();
            }
            return Ok(tickets);
        }

        //total income
        [HttpGet]
        [Route("get-total-income")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<int>> GetTotalIncome()
        {
            var tickets = await _ticketService.GetTotalIncome();
            return Ok(tickets);
        }


        [HttpGet]
        [Route("get-tickets-by-schedule")]
        //[Authorize(Roles = "Customer, Manager, Staff")]
        public async Task<ActionResult<IEnumerable<TicketDTO>>> GetTickets(int scheduleId)
        {
            var tickets = await _ticketService.GetTicketsBySchedule(scheduleId);
            if (tickets == null)
            {
                return NotFound();
            }
            return Ok(tickets);
        }

        [HttpPut("{id}")]
        //[Authorize(Roles = "Customer")]
        public async Task<IActionResult> OrderTicket(int id, int cusId)
        {
            try
            {
                await _ticketService.OrderTicket(id, cusId);
                await _hubContext.Clients.All.SendAsync("orderTicket");
                return NoContent();
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }

        [HttpPut]
        [Route("cancel-ticket")]
        public async Task<IActionResult> CancelTicket(int cusId)
        {
            try
            {
                await _ticketService.CancelTicket(cusId);
                await _hubContext.Clients.All.SendAsync("cancelTicket");
                return NoContent();
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("get-tickets-ordering")]
        //[Authorize(Roles = "Customer")]
        public async Task<ActionResult<IEnumerable<TicketDTO>>> GetTicketsOrdering(int cusId)
        {
            var tickets = await _ticketService.GetTicketsOrdering(cusId);
            if (tickets == null)
            {
                return NotFound();
            }
            return Ok(tickets);
        }

        [HttpPut]
        [Route("booking-tickets")]
        //[Authorize(Roles = "Customer")]
        public async Task<IActionResult> BookingTickets(int cusId, int scheduleId)
        {
            try
            {
                await _ticketService.BookingTickets(cusId, scheduleId);
                await _hubContext.Clients.All.SendAsync("bookingTicket");
                return NoContent();
            }
            catch (InvalidOperationException e)
            {
                return NotFound(e.Message);
            }
        }
    }

/*    public class TicketsController : ControllerBase
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

        [HttpGet]
        [Route("get-tickets-ordering")]
        public async Task<ActionResult<IEnumerable<TicketDTO>>> GetTicketsOrdering()
        {
            var tickets = await _unitOfWork.TicketRepository.GetTicketsOrdering();
            if(tickets == null)
            {
                   return NotFound();
            }
            var ticketDTOs = _mapper.Map<List<TicketDTO>>(tickets);
           
            return ticketDTOs;
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
    }*/
}
