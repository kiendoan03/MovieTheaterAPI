using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Humanizer.Localisation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
    public class StaffsController : ControllerBase
    {
        private readonly IStaffService _staffService;

        public StaffsController(IStaffService staffService)
        {
            _staffService = staffService;
        }

        [HttpGet]
        [Authorize(Roles = "Manager, Staff")]
        public async Task<ActionResult<IEnumerable<StaffDTO>>> GetStaffs()
        {
            var staffs = await _staffService.GetAllStaffs();
            return Ok(staffs);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<StaffDTO>> GetStaff(int id)
        {
            var staff = await _staffService.GetStaffById(id);
            if (staff == null)
            {
                return NotFound();
            }
            return Ok(staff);
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<StaffDTO>> PostStaff([FromForm]StaffDTO staff, IFormFile file)
        {
            var newStaff = await _staffService.CreateStaff(staff, file);
            return CreatedAtAction(nameof(GetStaff), new { id = newStaff.Id }, newStaff);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> PutStaff([FromForm] StaffDTO staff,int id, IFormFile? file)
        {
            try
            {
                await _staffService.UpdateStaff( staff,id, file);
                return NoContent();
            }
            catch (ArgumentException)
            {
                return BadRequest("Id mismatch");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteStaff(int id)
        {
            await _staffService.DeleteStaff(id);
            return NoContent();
        }


    }

/*    [Route("api/[controller]")]
    [ApiController]
    public class StaffsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StaffsController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        // GET: api/Staffs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StaffDTO>>> GetStaffs()
        {
            var staffs = await _unitOfWork.StaffRepository.GetAll();
            return _mapper.Map<List<StaffDTO>>(staffs);
        }

        // GET: api/Staffs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StaffDTO>> GetStaff(int id)
        {
            var staff = await _unitOfWork.StaffRepository.GetById(id);

            if (staff == null)
            {
                return NotFound();
            }

            return _mapper.Map<StaffDTO>(staff);
        }

        // PUT: api/Staffs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStaff(int id, StaffDTO staff)
        {
            if (id != staff.Id)
            {
                return BadRequest();
            }
            var updatedStaff = _mapper.Map<Staff>(staff);

            await _unitOfWork.StaffRepository.Update(updatedStaff);

            try
            {
                await _unitOfWork.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (StaffExists(id))
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

        // POST: api/Staffs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<StaffDTO>> PostStaff(StaffDTO staff)
        {
            var newStaff = _mapper.Map<Staff>(staff);
            await _unitOfWork.StaffRepository.Add(newStaff);
            await _unitOfWork.Save();

            return CreatedAtAction("GetStaff", new { id = newStaff.Id }, staff);
        }

        [HttpGet]
        [Route("login")]
        public async Task<ActionResult<StaffDTO>> Login(string username, string password)
        {
            var staff = await _unitOfWork.StaffRepository.Login(username, password);
            if (staff == null)
            {
                return NotFound();
            }
            var staffDTO = _mapper.Map<StaffDTO>(staff);
            staffDTO.Token = GenerateJwtToken(staff);
            return Ok(staffDTO);
        }

        private string GenerateJwtToken(Staff staff)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("super secret key");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", staff.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        // DELETE: api/Staffs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStaff(int id)
        {
            var staff = await _unitOfWork.StaffRepository.GetById(id);
            if (staff == null)
            {
                return NotFound();
            }

            await _unitOfWork.StaffRepository.Delete(staff);
            await _unitOfWork.Save();

            return NoContent();
        }

        private bool StaffExists(int id)
        {
            return _unitOfWork.StaffRepository.IsExists(id).Result;
        }
    }*/
}
