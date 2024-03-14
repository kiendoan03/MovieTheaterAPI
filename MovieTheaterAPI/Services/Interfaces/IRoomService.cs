using MovieTheaterAPI.DTOs;
using MovieTheaterAPI.Entities;

namespace MovieTheaterAPI.Services.Interfaces
{
    public interface IRoomService
    {
        Task<IEnumerable<RoomDTO>> GetAllRooms();
        Task<RoomDTO> GetRoomById(int id);
        Task<IEnumerable<SeatDTO>> GetSeatsByRoom(int roomId);
        Task UpdateRoom(int id, RoomDTO roomDTO);
        Task<RoomDTO> CreateRoom(RoomDTO roomDTO);
        Task DeleteRoom(int id);
    }
}
