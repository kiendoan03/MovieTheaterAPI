using AutoMapper;
using MovieTheaterAPI.DTOs;
using MovieTheaterAPI.Entities;
using MovieTheaterAPI.Repository.Interfaces;
using MovieTheaterAPI.Services.Interfaces;

namespace MovieTheaterAPI.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ScheduleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ScheduleDTO> CreateSchedule(ScheduleDTO schedule)
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
                    throw new Exception("Schedule is conflicted with existing schedules");
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
            return schedule;
        }

        public async Task DeleteSchedule(int id)
        {
            var schedule = await _unitOfWork.ScheduleRepository.GetById(id);
            await _unitOfWork.ScheduleRepository.Delete(schedule);
            await _unitOfWork.Save();
        }

        public async Task<IEnumerable<ScheduleDTO>> GetAllSchedules()
        {
            var schedules = await _unitOfWork.ScheduleRepository.GetAll();
            var scheduleDTO = _mapper.Map<IEnumerable<ScheduleDTO>>(schedules);
            return scheduleDTO;
        }

        public async Task<ScheduleDTO> GetScheduleById(int id)
        {
            var schedule = await _unitOfWork.ScheduleRepository.GetById(id);
            return _mapper.Map<ScheduleDTO>(schedule);
        }

        public async Task<IEnumerable<ScheduleDTO>> GetSchedulesByMovie(int movieId)
        {
            var schedules = await _unitOfWork.ScheduleRepository.GetSchedulesByMovie(movieId);
            return _mapper.Map<IEnumerable<ScheduleDTO>>(schedules);
        }

        public async Task<IEnumerable<ScheduleDTO>> GetSchedulesWithMovieRoom()
        {
            var schedules = await _unitOfWork.ScheduleRepository.GetSchedulesWithMovieRoom();
            return _mapper.Map<IEnumerable<ScheduleDTO>>(schedules);
        }

        public async Task<ScheduleDTO> GetScheduleWithDetail(int scheduleId)
        {
            var schedule = await _unitOfWork.ScheduleRepository.GetScheduleWithDetail(scheduleId);
            return _mapper.Map<ScheduleDTO>(schedule);
        }

        public async Task UpdateSchedule(int id, ScheduleDTO scheduleDTO)
        {
            if (id != scheduleDTO.Id)
            {
                throw new Exception("Id is not matching");
            }
            var schedule = _mapper.Map<Schedule>(scheduleDTO);

            var movie = await _unitOfWork.MovieRepository.GetById(schedule.MovieId);
            var length = movie.Length;
            int startMinutes = schedule.StartTime?.Hour * 60 + schedule.StartTime?.Minute ?? 0;
            int endMinutes = startMinutes + length + 15;
            int endHour = endMinutes / 60;
            int endMinute = endMinutes % 60;
            var endTime = new TimeOnly(endHour, endMinute);
            schedule.EndTime = endTime;

            var existingSchedules = await _unitOfWork.ScheduleRepository.GetSchedulesByDateAndRoomExceptOne(schedule.RoomId, schedule.ScheduleDate, schedule.Id);
            foreach (var existingSchedule in existingSchedules)
            {
                if (schedule.StartTime >= existingSchedule.StartTime && schedule.StartTime < existingSchedule.EndTime ||
                    schedule.EndTime > existingSchedule.StartTime && schedule.EndTime <= existingSchedule.EndTime)
                {
                    throw new Exception("Schedule is conflicted with existing schedules");
                }
            }

            var checkTickets = await _unitOfWork.TicketRepository.CheckTicket(schedule.Id);
                if (!checkTickets.Any())
                {
                  /*  //var oldSchedule = await _unitOfWork.ScheduleRepository.GetById(schedule.Id);

                    //if (oldSchedule.RoomId == schedule.RoomId)
                    //{
                    //    await _unitOfWork.ScheduleRepository.Update(schedule);
                    //    await _unitOfWork.Save();
                    //}
                    //else
                    //{*/

                        var tickets = await _unitOfWork.TicketRepository.GetTicketsByScheduleToDelete(schedule.Id);
                        foreach (var ticket in tickets)
                        {
                            await _unitOfWork.TicketRepository.Delete(ticket);
                        }

                        await _unitOfWork.ScheduleRepository.Update(schedule);
                        await _unitOfWork.Save();

                        var seats = await _unitOfWork.SeatRepository.GetSeatsByRoomId(schedule.RoomId);

                            foreach (var seat in seats)
                            {
                                var newTicket = new Ticket
                                {
                                    ScheduleId = schedule.Id,
                                    SeatId = seat.Id,
                                    FinalPrice = seat.SeatType.Price,
                                    status = 0
                                };
                                await _unitOfWork.TicketRepository.Add(newTicket);
                                await _unitOfWork.Save();
                            }
                    //}
                }
                else
                {
                    throw new Exception("Cannot update schedule because tickets have been ordered");
                }
           
        }
    }
}
