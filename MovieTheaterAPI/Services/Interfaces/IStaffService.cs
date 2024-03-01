using MovieTheaterAPI.DTOs;
using MovieTheaterAPI.Entities;

namespace MovieTheaterAPI.Services.Interfaces
{
    public interface IStaffService
    {
        Task<IEnumerable<StaffDTO>> GetAllStaffs();
        Task<StaffDTO> GetStaffById(int id);
        Task<StaffDTO> CreateStaff(StaffDTO staff);
        Task UpdateStaff(int id, StaffDTO staff);
        Task DeleteStaff(int id);
    }
}
