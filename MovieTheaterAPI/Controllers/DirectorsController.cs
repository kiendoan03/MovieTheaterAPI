using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTheaterAPI.DAL;
using MovieTheaterAPI.DTOs;
using MovieTheaterAPI.Entities;
using MovieTheaterAPI.Repository;
using MovieTheaterAPI.Services.Interfaces;

namespace MovieTheaterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DirectorsController : ControllerBase
    {
        private readonly IDirectorService _directorService;

        public DirectorsController(IDirectorService directorService)
        {
            _directorService = directorService;
        }

        [HttpGet]
        //[Authorize (Roles = "Staff, Manager")]
        public async Task<ActionResult<IEnumerable<DirectorDTO>>> GetDirectors()
        {
            var directors = await _directorService.GetAllDirectors();
            return Ok(directors);
        }

        [HttpGet("{id}")]
        //[Authorize(Roles = "Manager")]
        public async Task<ActionResult<DirectorDTO>> GetDirector(int id)
        {
            var director = await _directorService.GetDirectorById(id);
            if (director == null)
            {
                return NotFound();
            }
            return Ok(director);
        }

        [HttpPost]
        //[Authorize(Roles = "Manager")]
        public async Task<ActionResult<DirectorDTO>> PostDirector([FromForm]DirectorDTO director, IFormFile file)
        {
            var newDirector = await _directorService.CreateDirector(director, file);
            return CreatedAtAction(nameof(GetDirector), new { id = newDirector.Id }, newDirector);
        }

        [HttpPut("{id}")]
        //[Authorize(Roles = "Manager")]
        public async Task<IActionResult> PutDirector([FromForm] DirectorDTO director,int id, IFormFile? file)
        {
            try
            {
                await _directorService.UpdateDirector( director,id, file);
                return NoContent();
            }
            catch (ArgumentException)
            {
                return BadRequest("Id mismatch");
            }
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteDirector(int id)
        {
            await _directorService.DeleteDirector(id);
            return NoContent();
        }
    }



    /*    [Route("api/[controller]")]
        [ApiController]
        public class DirectorsController : ControllerBase
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;

            public DirectorsController(IMapper mapper, IUnitOfWork unitOfWork)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
            }

            // GET: api/Directors
            [HttpGet]
            public async Task<ActionResult<IEnumerable<DirectorDTO>>> GetDirectors()
            {
                var directors = await _unitOfWork.DirectorRepository.GetAll();
                return _mapper.Map<List<DirectorDTO>>(directors);
            }

            // GET: api/Directors/5
            [HttpGet("{id}")]
            public async Task<ActionResult<DirectorDTO>> GetDirector(int id)
            {
                var director = await _unitOfWork.DirectorRepository.GetById(id);

                if (director == null)
                {
                    return NotFound();
                }

                return _mapper.Map<DirectorDTO>(director);
            }

            // PUT: api/Directors/5
            // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
            [HttpPut("{id}")]
            public async Task<IActionResult> PutDirector(int id, DirectorDTO director)
            {
                if (id != director.Id)
                {
                    return BadRequest();
                }

                var updatedDirector = _mapper.Map<Director>(director);

                await _unitOfWork.DirectorRepository.Update(updatedDirector);

                try
                {
                    await _unitOfWork.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (DirectorExists(id))
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

            // POST: api/Directors
            // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
            [HttpPost]
            public async Task<ActionResult<DirectorDTO>> PostDirector(DirectorDTO director)
            {
                var newDirector = _mapper.Map<Director>(director);

                await _unitOfWork.DirectorRepository.Add(newDirector);
                await _unitOfWork.Save();

                return CreatedAtAction("GetDirector", new { id = newDirector.Id }, director);
            }

            // DELETE: api/Directors/5
            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteDirector(int id)
            {
                var director = await _unitOfWork.DirectorRepository.GetById(id);
                if (director == null)
                {
                    return NotFound();
                }

                await _unitOfWork.DirectorRepository.Delete(director);
                await _unitOfWork.Save();

                return NoContent();
            }

            private bool DirectorExists(int id)
            {
                return _unitOfWork.DirectorRepository.IsExists(id).Result;
            }
        }*/
}
