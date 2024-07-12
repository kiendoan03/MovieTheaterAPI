using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Humanizer.Localisation;
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
    public class RoomsController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomsController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet]
        [Authorize (Roles = "Staff, Manager, Customer")]
        public async Task<ActionResult<IEnumerable<RoomDTO>>> GetRooms()
        {
            var rooms = await _roomService.GetAllRooms();
            rooms = rooms.Reverse().ToList();
            return Ok(rooms);
        }

        [HttpGet ("{id}")]
        [Authorize(Roles = "Manager, Staff")]
        public async Task<ActionResult<RoomDTO>> GetRoom(int id)
        {
            var room = await _roomService.GetRoomById(id);
            if (room == null)
            {
                return NotFound();
            }
            return Ok(room);
        }

        [HttpGet]
        [Route("GetSeatsbyRoomId/{id}")]
        public async Task<ActionResult<IEnumerable<SeatDTO>>> GetSeatsbyRoomId(int id)
        {
            var seats = await _roomService.GetSeatsByRoom(id);
            return Ok(seats);
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<RoomDTO>> PostRoom(RoomDTO room)
        {
            var newRoom = await _roomService.CreateRoom(room);
            return CreatedAtAction(nameof(GetRoom), new { id = newRoom.Id }, newRoom);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> PutRoom(int id, RoomDTO room)
        {
            try
            {
                await _roomService.UpdateRoom(id, room);
                return NoContent();
            }
            catch (ArgumentException)
            {
                return BadRequest("Id mismatch");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            await _roomService.DeleteRoom(id);
            return NoContent();
        }


    }
        /*    [Route("api/[controller]")]
            [ApiController]
            public class RoomsController : ControllerBase
            {
                private readonly IUnitOfWork _unitOfWork;
                private readonly IMapper _mapper;

                public RoomsController(IMapper mapper, IUnitOfWork unitOfWork)
                {
                    _mapper = mapper;
                    _unitOfWork = unitOfWork;
                }

                // GET: api/Rooms
                [HttpGet]
                public async Task<ActionResult<IEnumerable<RoomDTO>>> GetRooms()
                {
                    var rooms = await _unitOfWork.RoomRepository.GetAll();

                    return _mapper.Map<List<RoomDTO>>(rooms);
                }

                // GET: api/Rooms/5
                [HttpGet("{id}")]
                public async Task<ActionResult<RoomDTO>> GetRoom(int id)
                {
                    var room = await _unitOfWork.RoomRepository.GetById(id);

                    if (room == null)
                    {
                        return NotFound();
                    }

                    return _mapper.Map<RoomDTO>(room);
                }

                // PUT: api/Rooms/5
                // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
                [HttpPut("{id}")]
                public async Task<IActionResult> PutRoom(int id, RoomDTO room)
                {
                    if (id != room.Id)
                    {
                        return BadRequest();
                    }
                    var updatedRoom = _mapper.Map<Room>(room);

                    await _unitOfWork.RoomRepository.Update(updatedRoom);

                    try
                    {
                        await _unitOfWork.Save();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (RoomExists(id))
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

                // POST: api/Rooms
                // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
                [HttpPost]
                public async Task<ActionResult<RoomDTO>> PostRoom(RoomDTO room)
                {
                    var newRoom = _mapper.Map<Room>(room);
                    await _unitOfWork.RoomRepository.Add(newRoom);
                    await _unitOfWork.Save();
                    if(room.RoomTypeId == 1)
                    {
                        for (int i = 1; i <= 6; i++)
                        {
                            for(int j = 1; j <= 12; j++)
                            {
                                if(i <= 3)
                                {
                                    Seat seat = new Seat();
                                    seat.RoomId = newRoom.Id;
                                    seat.Row = i;
                                    seat.Column = j;
                                    seat.SeatTypeId = 1;
                                    await _unitOfWork.SeatRepository.Add(seat);
                                    await _unitOfWork.Save();
                                }
                                else if(i <= 5 && i > 3)
                                {
                                    Seat seat = new Seat();
                                    seat.RoomId = newRoom.Id;
                                    seat.Row = i;
                                    seat.Column = j;
                                    seat.SeatTypeId = 2;
                                    await _unitOfWork.SeatRepository.Add(seat);
                                    await _unitOfWork.Save();
                                }
                                else if(i == 6 && j <= 6)
                                { 
                                    Seat seat = new Seat();
                                    seat.RoomId = newRoom.Id;
                                    seat.Row = i;
                                    seat.Column = j;
                                    seat.SeatTypeId = 3;
                                    await _unitOfWork.SeatRepository.Add(seat);
                                    await _unitOfWork.Save();
                                }

                            }
                        } 
                    }
                    else if(room.RoomTypeId == 2)
                    {
                        for (int i = 1; i <= 4; i++)
                        {
                            for (int j = 1; j <= 6; j++)
                            {
                                    Seat seat = new Seat();
                                    seat.RoomId = newRoom.Id;
                                    seat.Row = i;
                                    seat.Column = j;
                                    seat.SeatTypeId = 4;
                                    await _unitOfWork.SeatRepository.Add(seat);
                                    await _unitOfWork.Save();
                            }
                        }
                    }


                    return CreatedAtAction("GetRoom", new { id = newRoom.Id }, room);
                }

                // DELETE: api/Rooms/5
                [HttpDelete("{id}")]
                public async Task<IActionResult> DeleteRoom(int id)
                {
                    var room = await _unitOfWork.RoomRepository.GetById(id);
                    if (room == null)
                    {
                        return NotFound();
                    }
                    await _unitOfWork.RoomRepository.Delete(room);
                    await _unitOfWork.Save();

                    return NoContent();
                }

                private bool RoomExists(int id)
                {
                    return _unitOfWork.RoomRepository.IsExists(id).Result;
                }
            }*/
    }
