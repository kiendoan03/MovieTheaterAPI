using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTheaterAPI.DAL;
using MovieTheaterAPI.DTOs;
using MovieTheaterAPI.Entities;
using MovieTheaterAPI.Repository;
using MovieTheaterAPI.Services;
using MovieTheaterAPI.Services.Interfaces;

namespace MovieTheaterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
     public class SchedulesController : ControllerBase
     {
        private readonly IScheduleService _scheduleService;

        public SchedulesController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        [HttpGet]
        [Authorize(Roles = "Manager, Staff")]
        public async Task<ActionResult<IEnumerable<ScheduleDTO>>> GetSchedules()
        {
            var schedules = await _scheduleService.GetAllSchedules();
            return Ok(schedules);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Manager, Staff")]
        public async Task<ActionResult<ScheduleDTO>> GetSchedule(int id)
        {
            var schedule = await _scheduleService.GetScheduleById(id);
            if (schedule == null)
            {
                return NotFound();
            }
            return Ok(schedule);
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<ScheduleDTO>> PostSchedule(ScheduleDTO schedule)
        {
            var newSchedule = await _scheduleService.CreateSchedule(schedule);
            return CreatedAtAction(nameof(GetSchedule), new { id = newSchedule.Id }, newSchedule);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> PutSchedule(int id, ScheduleDTO schedule)
        {
            try
            {
                await _scheduleService.UpdateSchedule(id, schedule);
                return NoContent();
            }
            catch (ArgumentException)
            {
                return BadRequest("Id mismatch");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteSchedule(int id)
        {
            await _scheduleService.DeleteSchedule(id);
            return NoContent();
        }

        [HttpGet]
        [Route("get-schedules-by-movie")]
        public async Task<ActionResult<IEnumerable<ScheduleDTO>>> GetSchedulesByMovie(int movieId)
        {
            var schedules = await _scheduleService.GetSchedulesByMovie(movieId);
            if (schedules == null)
            {
                return NotFound();
            }
            return Ok(schedules);
        }

        [HttpGet]
        [Route("get-schedules-with-movie-room")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ScheduleDTO>>> GetSchedulesWithMovieRoom()
        {
            var schedules = await _scheduleService.GetSchedulesWithMovieRoom();
            if (schedules == null)
            {
                return NotFound();
            }
            return Ok(schedules);
        }

        [HttpGet]
        [Route("get-schedule-with-detail")]
        //[Authorize(Roles = "Customer")]
        public async Task<ActionResult<ScheduleDTO>> GetScheduleWithDetail(int scheduleId)
        {
            var schedules = await _scheduleService.GetScheduleWithDetail(scheduleId);
            if (schedules == null)
            {
                return NotFound();
            }
            return Ok(schedules);
        }

     }

/*    [Route("api/[controller]")]
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
        public async Task<ActionResult<IEnumerable<Schedule>>> GetSchedules()
        {
            var schedules = await _unitOfWork.ScheduleRepository.GetSchedulesDetails();
            return Ok(schedules);
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

            var movie = await _unitOfWork.MovieRepository.GetById(schedule.MovieId);
            var length = movie.Length;
            int startMinutes = newSchedule.StartTime?.Hour * 60 + newSchedule.StartTime?.Minute ?? 0;
            int endMinutes = startMinutes + length + 15;
            int endHour = endMinutes / 60;
            int endMinute = endMinutes % 60;
            var endTime = new TimeOnly(endHour, endMinute);
            newSchedule.EndTime = endTime;

            var existingSchedules = await _unitOfWork.ScheduleRepository.GetSchedulesByDateAndRoom(newSchedule.RoomId, newSchedule.ScheduleDate);
            foreach (var existingSchedule in existingSchedules)
            {
                if (newSchedule.StartTime >= existingSchedule.StartTime && newSchedule.StartTime < existingSchedule.EndTime ||
                    newSchedule.EndTime > existingSchedule.StartTime && newSchedule.EndTime <= existingSchedule.EndTime)
                {
                    return BadRequest("Schedule conflicts with existing schedules");
                }
            }

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

            return CreatedAtAction("GetSchedule", new { id = newSchedule.Id }, schedule);
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

        [HttpGet]
        [Route("get-schedules-by-movie")]
        public async Task<ActionResult<IEnumerable<Schedule>>> GetSchedulesByMovie(int movieId)
        {
            var schedules = await _unitOfWork.ScheduleRepository.GetSchedulesByMovie(movieId);
            if (schedules == null)
            {
                return NotFound();
            }
            return Ok(schedules);
        }


        private bool ScheduleExists(int id)
        {
            return _unitOfWork.ScheduleRepository.IsExists(id).Result;
        }


    }*/
}
