using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTheaterAPI.DAL;
using MovieTheaterAPI.Entities;
using MovieTheaterAPI.DTOs;
using MovieTheaterAPI.Repository;

namespace MovieTheaterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MoviesController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        // GET: api/Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieDTO>>> GetMovies()
        {
            var movies = await _unitOfWork.MovieRepository.GetAll();
           
            return _mapper.Map<List<MovieDTO>>(movies);
        }

        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDTO>> GetMovie(int id)
        {
            var movie = await _unitOfWork.MovieRepository.GetById(id);

            if (movie == null)
            {
                return NotFound();
            }

            return _mapper.Map<MovieDTO>(movie);
        }

        // PUT: api/Movies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, MovieDTO movie)
        {
            if (id != movie.Id)
            {
                return BadRequest();
            }

            var movieExists = await _unitOfWork.MovieRepository.GetMovieWithFk(id);

            _mapper.Map(movie, movieExists);
            await _unitOfWork.MovieRepository.Update(movieExists);

            try
            {
                await _unitOfWork.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (MovieExists(id))
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

        // POST: api/Movies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MovieDTO>> PostMovie([FromBody] MovieDTO movie)
        {
            var newMovie = _mapper.Map<Movie>(movie);
            
           
            await _unitOfWork.MovieRepository.Add(newMovie);
            await _unitOfWork.Save();

            return CreatedAtAction("GetMovie", new { id = newMovie.Id }, newMovie);
        }

        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _unitOfWork.MovieRepository.GetById(id);
            if (movie == null)
            {
                return NotFound();
            }

            
            await _unitOfWork.MovieRepository.Delete(movie);
            await _unitOfWork.Save();

            return NoContent();
        }

        private bool MovieExists(int id)
        {
            return _unitOfWork.MovieRepository.IsExists(id).Result;
        }
    }
}
