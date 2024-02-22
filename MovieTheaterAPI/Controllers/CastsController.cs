﻿using System;
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
    public class CastsController : ControllerBase
    {
        //private readonly MovieTheaterDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CastsController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            //_context = context;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        // GET: api/Casts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CastDTO>>> GetCasts()
        {
            //var casts = await _context.Casts.ToListAsync();
            var casts = await _unitOfWork.CastRepository.GetAll();
            return _mapper.Map<List<CastDTO>>(casts);
        }

        // GET: api/Casts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CastDTO>> GetCast(int id)
        {
            //var cast = await _context.Casts.FindAsync(id);
            var cast = await _unitOfWork.CastRepository.GetById(id);

            if (cast == null)
            {
                return NotFound();
            }

            return _mapper.Map<CastDTO>(cast);
        }

        // PUT: api/Casts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCast(int id, CastDTO cast)
        {
            if (id != cast.Id)
            {
                return BadRequest();
            }

            var updatedCast = _mapper.Map<Cast>(cast);

            //_context.Entry(updatedCast).State = EntityState.Modified;
            await _unitOfWork.CastRepository.Update(updatedCast);   

            try
            {
                //await _context.SaveChangesAsync();
                await _unitOfWork.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (CastExists(id))
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

        // POST: api/Casts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CastDTO>> PostCast(CastDTO cast)
        {
            var newCast = _mapper.Map<Cast>(cast);
            //_context.Casts.Add(newCast);
            //await _context.SaveChangesAsync();
            await _unitOfWork.CastRepository.Add(newCast);
            await _unitOfWork.Save();

            return CreatedAtAction("GetCast", new { id = newCast.Id }, newCast);
        }

        // DELETE: api/Casts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCast(int id)
        {
            //var cast = await _context.Casts.FindAsync(id);
            var cast = await _unitOfWork.CastRepository.GetById(id);
            if (cast == null)
            {
                return NotFound();
            }

            //_context.Casts.Remove(cast);
            //await _context.SaveChangesAsync();
            await _unitOfWork.CastRepository.Delete(cast);
            await _unitOfWork.Save();

            return NoContent();
        }

        private bool CastExists(int id)
        {
            //return _context.Casts.Any(e => e.Id == id);
            return _unitOfWork.CastRepository.IsExists(id).Result;
        }
    }
}
