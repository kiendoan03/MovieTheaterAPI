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

        public async Task<bool> CheckDuplicateStaff(string username, string email)
        {
            return await _unitOfWork.StaffRepository.CheckDuplicateStaff(username, email);
        }

        public async Task<bool> CheckDuplicateStaffExcept(string username, string email, int id)
        {
            return await _unitOfWork.StaffRepository.CheckDuplicateStaffExcept(username, email, id);
        }

        public async Task<StaffDTO> CreateStaff(StaffDTO staff, IFormFile file)
        {
            var newStaff = _mapper.Map<Staff>(staff);
           /* //await _unitOfWork.StaffRepository.Add(newStaff);
            //await _unitOfWork.Save();*/

            if(file.Length > 0)
            {
                var folderName = Path.Combine("wwwroot", "uploads", "images", "staffs");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                var fileName = Guid.NewGuid().ToString() + "_" + newStaff.UserName + ".png";
                var fullPath = Path.Combine(pathToSave, fileName);
                using(var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                newStaff.Image = "/uploads/images/staffs/" + fileName;
            }

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
            var staffDTO = _mapper.Map<IEnumerable<StaffDTO>>(staffs);
            foreach (var staff in staffs)
            { 
                var role = await _userManager.GetRolesAsync(staff);
                if (role.Count > 0)
                {
                        if (role[0] == "Manager")
                    {
                            staffDTO.FirstOrDefault(x => x.Id == staff.Id).StaffRole = 0;
                        }
                        else
                    {
                            staffDTO.FirstOrDefault(x => x.Id == staff.Id).StaffRole = 1;
                        }
                    }
            }
            return staffDTO;
        }

        public async Task<StaffDTO> GetStaffById(int id)
        {
            var staff = await _unitOfWork.StaffRepository.GetById(id);
            var staffDTO = _mapper.Map<StaffDTO>(staff);
            var role = await _userManager.GetRolesAsync(staff);
            if (role.Count > 0)
            {
                if (role[0] == "Manager")
                {
                    staffDTO.StaffRole = 0;
                }
                else
                {
                    staffDTO.StaffRole = 1;
                }
            }
            return staffDTO;
        }

        public async Task UpdateStaff( StaffDTO staff,int id, IFormFile? file)
        {
            if (id != staff.Id)
            {
                throw new ArgumentException("Id mismatch");
            }

            var oldStaff = await _unitOfWork.StaffRepository.GetById(id);

            if(file != null && file.Length > 0)
            {
                var folderName = Path.Combine("wwwroot", "uploads", "images", "staffs");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                var fileName = Guid.NewGuid().ToString() + "_" + staff.Username + ".png";
                var fullPath = Path.Combine(pathToSave, fileName);
                using(var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                staff.Image = "/uploads/images/staffs/" + fileName;
            }
            else
            {
                staff.Image = oldStaff.Image;
            }

            

            if (string.IsNullOrEmpty(staff.PasswordHash))
            {
                // If no new password provided, keep the old password
                staff.PasswordHash = oldStaff.PasswordHash;
                _mapper.Map(staff, oldStaff);
            }
            else
            { 
                _mapper.Map(staff, oldStaff);
                // If a new password is provided, hash it
                var newPasswordHash = _userManager.PasswordHasher.HashPassword(oldStaff, staff.PasswordHash);
                // Update the staff's password hash
                oldStaff.PasswordHash = newPasswordHash;
            }

            // Update the user
            await _userManager.UpdateAsync(oldStaff);
            // Remove all existing roles of the staff
            var userRoles = await _userManager.GetRolesAsync(oldStaff);
            await _userManager.RemoveFromRolesAsync(oldStaff, userRoles);

            // Update roles if necessary
            if (staff.StaffRole == 0)
            {
                // Add user to Manager role
                await _userManager.AddToRoleAsync(oldStaff, "Manager");
            }
            else
            {
                // Add user to Staff role
                await _userManager.AddToRoleAsync(oldStaff, "Staff");
            }
        }
    }
}
