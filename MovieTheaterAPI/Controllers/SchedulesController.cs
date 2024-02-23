using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class SchedulesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SchedulesController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        // GET: api/Schedules
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ScheduleDTO>>> GetSchedules()
        {
            var schedules = await _unitOfWork.ScheduleRepository.GetAll();
            return _mapper.Map<List<ScheduleDTO>>(schedules);
        }

        // GET: api/Schedules/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ScheduleDTO>> GetSchedule(int id)
        {
            var schedule = await _unitOfWork.ScheduleRepository.GetById(id);

            if (schedule == null)
            {
                return NotFound();
            }

            return _mapper.Map<ScheduleDTO>(schedule);
        }

        // PUT: api/Schedules/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSchedule(int id, ScheduleDTO schedule)
        {
            if (id != schedule.Id)  
            {
                return BadRequest();
            }
            var updatedSchedule = _mapper.Map<Schedule>(schedule);
            await _unitOfWork.ScheduleRepository.Update(updatedSchedule);

            try
            {
                await _unitOfWork.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (ScheduleExists(id))
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

        // POST: api/Schedules
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ScheduleDTO>> PostSchedule(ScheduleDTO schedule)
        {
            var newSchedule = _mapper.Map<Schedule>(schedule);
            await _unitOfWork.ScheduleRepository.Add(newSchedule);
            await _unitOfWork.Save();

            var seats = await _unitOfWork.SeatRepository.GetSeatsByRoomId(schedule.RoomId);

            foreach (var seat in seats)
            {
                var ticket = new Ticket
                {
                    ScheduleId = newSchedule.Id,
                    SeatId = seat.Id,
                    FinalPrice = seat.SeatType.Price,
                    status = 0
                };
                await _unitOfWork.TicketRepository.Add(ticket);
                await _unitOfWork.Save();
            }

            return CreatedAtAction("GetSchedule", new { id = newSchedule.Id }, newSchedule);
        }

        // DELETE: api/Schedules/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchedule(int id)
        {
            var schedule = await _unitOfWork.ScheduleRepository.GetById(id);
            if (schedule == null)
            {
                return NotFound();
            }

            await _unitOfWork.ScheduleRepository.Delete(schedule);
            await _unitOfWork.Save();

            return NoContent();
        }

        private bool ScheduleExists(int id)
        {
            return _unitOfWork.ScheduleRepository.IsExists(id).Result;
        }
    }
}
