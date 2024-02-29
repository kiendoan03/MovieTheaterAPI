using AutoMapper;
using MovieTheaterAPI.DTOs;
using MovieTheaterAPI.Entities;
using MovieTheaterAPI.Repository;
using MovieTheaterAPI.Services.Interfaces;

namespace MovieTheaterAPI.Services
{
    public class RoomService : IRoomService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RoomService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<RoomDTO> CreateRoom(RoomDTO roomDTO)
        {
            var newRoom = _mapper.Map<Room>(roomDTO);
            await _unitOfWork.RoomRepository.Add(newRoom);
            await _unitOfWork.Save();
            if (roomDTO.RoomTypeId == 1)
            {
                for (int i = 1; i <= 6; i++)
                {
                    for (int j = 1; j <= 12; j++)
                    {
                        if (i <= 3)
                        {
                            Seat seat = new Seat();
                            seat.RoomId = newRoom.Id;
                            seat.Row = i;
                            seat.Column = j;
                            seat.SeatTypeId = 1;
                            await _unitOfWork.SeatRepository.Add(seat);
                            await _unitOfWork.Save();
                        }
                        else if (i <= 5 && i > 3)
                        {
                            Seat seat = new Seat();
                            seat.RoomId = newRoom.Id;
                            seat.Row = i;
                            seat.Column = j;
                            seat.SeatTypeId = 2;
                            await _unitOfWork.SeatRepository.Add(seat);
                            await _unitOfWork.Save();
                        }
                        else if (i == 6 && j <= 6)
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
            else if (roomDTO.RoomTypeId == 2)
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
            return roomDTO;
        }

        public async Task DeleteRoom(int id)
        {
            var room = await _unitOfWork.RoomRepository.GetById(id);
            await _unitOfWork.RoomRepository.Delete(room);
            await _unitOfWork.Save();
        }

        public async Task<IEnumerable<RoomDTO>> GetAllRooms()
        {
            var rooms = await _unitOfWork.RoomRepository.GetAll();
            return _mapper.Map<List<RoomDTO>>(rooms);
        }

        public async Task<RoomDTO> GetRoomById(int id)
        {
            var room = await _unitOfWork.RoomRepository.GetById(id);
            return _mapper.Map<RoomDTO>(room);
        }

        public async Task UpdateRoom(int id, RoomDTO roomDTO)
        {
            if (id != roomDTO.Id)
            {
                throw new Exception("Id is not matching");
            }
            var updatedRoom = _mapper.Map<Room>(roomDTO);
            await _unitOfWork.RoomRepository.Update(updatedRoom);
            await _unitOfWork.Save();
        }
    }
}
