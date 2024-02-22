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
    public class GenresController : ControllerBase
    {
        //private readonly MovieTheaterDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GenresController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            //_context = context;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        // GET: api/Genres
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GenreDTO>>> GetGenres()
        {
            //var genres = await _context.Genres.ToListAsync();
            var genres = await _unitOfWork.GenreRepository.GetAll();
            return _mapper.Map<List<GenreDTO>>(genres);
        }

        // GET: api/Genres/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GenreDTO>> GetGenre(int id)
        {
            //var genre = await _context.Genres.FindAsync(id);
            var genre = await _unitOfWork.GenreRepository.GetById(id);

            if (genre == null)
            {
                return NotFound();
            }

            return _mapper.Map<GenreDTO>(genre);
        }

        // PUT: api/Genres/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGenre(int id, GenreDTO genre)
        {
            if (id != genre.Id)
            {
                return BadRequest();
            }

            var update_genre = _mapper.Map<Genre>(genre);

            //_context.Entry(update_genre).State = EntityState.Modified;
            await _unitOfWork.GenreRepository.Update(update_genre);

            try
            {
                //await _context.SaveChangesAsync();
                await _unitOfWork.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (GenreExists(id))
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

        // POST: api/Genres
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GenreDTO>> PostGenre(GenreDTO genre)
        {
            var new_genre = _mapper.Map<Genre>(genre);
            //_context.Genres.Add(new_genre);
            //await _context.SaveChangesAsync();
            await _unitOfWork.GenreRepository.Add(new_genre);
            await _unitOfWork.Save();

            return CreatedAtAction("GetGenre", new { id = new_genre.Id }, new_genre);
        }

        // DELETE: api/Genres/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            //var genre = await _context.Genres.FindAsync(id);
            var genre = await _unitOfWork.GenreRepository.GetById(id);
            if (genre == null)
            {
                return NotFound();
            }

            //_context.Genres.Remove(genre);
            //await _context.SaveChangesAsync();
            await _unitOfWork.GenreRepository.Delete(genre);
            await _unitOfWork.Save();

            return NoContent();
        }

        private bool GenreExists(int id)
        {
            //return _context.Genres.Any(e => e.Id == id);
            return _unitOfWork.GenreRepository.IsExists(id).Result;
        }
    }
}
