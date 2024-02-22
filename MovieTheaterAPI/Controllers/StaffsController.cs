using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTheaterAPI.DAL;
using MovieTheaterAPI.DTOs;
using MovieTheaterAPI.Entities;
using MovieTheaterAPI.Repository;

namespace MovieTheaterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffsController : ControllerBase
    {
        //private readonly MovieTheaterDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StaffsController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            //_context = context;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        // GET: api/Staffs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StaffDTO>>> GetStaffs()
        {
            //var staffs = await _context.Staffs.ToListAsync();
            var staffs = await _unitOfWork.StaffRepository.GetAll();
            return _mapper.Map<List<StaffDTO>>(staffs);
        }

        // GET: api/Staffs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StaffDTO>> GetStaff(int id)
        {
            //var staff = await _context.Staffs.FindAsync(id);
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

            //_context.Entry(updatedStaff).State = EntityState.Modified;
            await _unitOfWork.StaffRepository.Update(updatedStaff);

            try
            {
                //await _context.SaveChangesAsync();
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
            //_context.Staffs.Add(newStaff);
            //await _context.SaveChangesAsync();
            await _unitOfWork.StaffRepository.Add(newStaff);
            await _unitOfWork.Save();

            return CreatedAtAction("GetStaff", new { id = newStaff.Id }, newStaff);
        }

        // DELETE: api/Staffs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStaff(int id)
        {
            //var staff = await _context.Staffs.FindAsync(id);
            var staff = await _unitOfWork.StaffRepository.GetById(id);
            if (staff == null)
            {
                return NotFound();
            }

            //_context.Staffs.Remove(staff);
            //await _context.SaveChangesAsync();
            await _unitOfWork.StaffRepository.Delete(staff);
            await _unitOfWork.Save();

            return NoContent();
        }

        private bool StaffExists(int id)
        {
            return _unitOfWork.StaffRepository.IsExists(id).Result;
        }
    }
}
