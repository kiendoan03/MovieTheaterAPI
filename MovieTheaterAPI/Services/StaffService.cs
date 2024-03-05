using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieTheaterAPI.DTOs;
using MovieTheaterAPI.Entities;
using MovieTheaterAPI.Repository.Interfaces;
using MovieTheaterAPI.Services.Interfaces;
using System.Security.AccessControl;

namespace MovieTheaterAPI.Services
{
    public class StaffService : IStaffService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly UserManager<User> _userManager;

        public StaffService(IUnitOfWork unitOfWork, IMapper mapper, ITokenService tokenService, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tokenService = tokenService;
            _userManager = userManager;
        }

        public async Task<StaffDTO> CreateStaff([FromBody] StaffDTO staff)
        {
            var newStaff = _mapper.Map<Staff>(staff);
           /* //await _unitOfWork.StaffRepository.Add(newStaff);
            //await _unitOfWork.Save();*/

            var createdUser = await _userManager.CreateAsync(newStaff, staff.PasswordHash);
            if (!createdUser.Succeeded)
            {
                throw new AccessViolationException("User creation failed");
            }
            else
            {
                if(staff.StaffRole == 0)
                {
                    var role = await _userManager.AddToRoleAsync(newStaff, "Manager");
                    if(!role.Succeeded)
                    {
                        throw new AccessViolationException("Role creation failed");
                    }else
                    {
                        var staffDto = _mapper.Map<StaffDTO>(newStaff);
                        staffDto.Token = await _tokenService.CreateTokenAsync(newStaff);
                        return staffDto;
                    }
                }else
                {
                    var role = await _userManager.AddToRoleAsync(newStaff, "Staff");
                    if (!role.Succeeded)
                    {
                        throw new AccessViolationException("Role creation failed");
                    }
                    else
                    {
                        var staffDto = _mapper.Map<StaffDTO>(newStaff);
                        staffDto.Token = await _tokenService.CreateTokenAsync(newStaff);
                        return staffDto;
                    }
                }
               
            }
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
