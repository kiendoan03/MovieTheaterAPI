using MovieTheaterAPI.DTOs;
using MovieTheaterAPI.Entities;

namespace MovieTheaterAPI.Services.Interfaces
{
    public interface IStaffService
    {
        Task<IEnumerable<StaffDTO>> GetAllStaffs();
        Task<StaffDTO> GetStaffById(int id);
        Task<StaffDTO> CreateStaff(StaffDTO staff, IFormFile file);
        Task UpdateStaff( StaffDTO staff,int id, IFormFile? file);
        Task DeleteStaff(int id);
        //CheckDuplicateStaff
        Task<bool> CheckDuplicateStaff(string username, string email);
        //CheckDuplicateStaffExcept
        Task<bool> CheckDuplicateStaffExcept(string username, string email, int id);
    }
}
