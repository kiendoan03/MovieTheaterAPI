using AutoMapper;
using MovieTheaterAPI.DTOs;
using MovieTheaterAPI.Entities;
using MovieTheaterAPI.Repository;
using MovieTheaterAPI.Services.Interfaces;
using System.Security.AccessControl;

namespace MovieTheaterAPI.Services
{
    public class StaffService : IStaffService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StaffService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<StaffDTO> CreateStaff(StaffDTO staff)
        {
            var newStaff = _mapper.Map<Staff>(staff);
            await _unitOfWork.StaffRepository.Add(newStaff);
            await _unitOfWork.Save();
            return staff;
        }

        public async Task DeleteStaff(int id)
        {
           var staff = await _unitOfWork.StaffRepository.GetById(id);
            await _unitOfWork.StaffRepository.Delete(staff);
            await _unitOfWork.Save();
        }

        public async Task<IEnumerable<StaffDTO>> GetAllStaffs()
        {
            var staffs = await _unitOfWork.StaffRepository.GetAll();
            return _mapper.Map<IEnumerable<StaffDTO>>(staffs);
        }

        public async Task<StaffDTO> GetStaffById(int id)
        {
            var staff = await _unitOfWork.StaffRepository.GetById(id);
            return _mapper.Map<StaffDTO>(staff);
        }

        public async Task UpdateStaff(int id, StaffDTO staff)
        {
            if (id != staff.Id)
            {
                throw new ArgumentException("Id mismatch");
            }

            var updatedStaff = _mapper.Map<Staff>(staff);
            await _unitOfWork.StaffRepository.Update(updatedStaff);
            await _unitOfWork.Save();
        }
    }
}
