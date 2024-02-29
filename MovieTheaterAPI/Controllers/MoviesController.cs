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
using MovieTheaterAPI.Services.Interfaces;
using Humanizer.Localisation;
using MovieTheaterAPI.Services;

namespace MovieTheaterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieDTO>>> GetMovies()
        {
            var movies = await _movieService.GetAllMovies();
            return Ok(movies);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDTO>> GetMovie(int id)
        {
            var movie = await _movieService.GetMovieById(id);
            if (movie == null)
            {
                return NotFound();
            }
            return Ok(movie);
        }

        [HttpPost]
        public async Task<ActionResult<MovieDTO>> PostMovie(MovieDTO movie)
        {
            var newMovie = await _movieService.CreateMovie(movie);
            return CreatedAtAction(nameof(GetMovie), new { id = newMovie.Id }, newMovie);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, MovieDTO movie)
        {
            try
            {
                await _movieService.UpdateMovie(id, movie);
                return NoContent();
            }
            catch (ArgumentException)
            {
                return BadRequest("Id mismatch");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            await _movieService.DeleteMovie(id);
            return NoContent();
        }

        [HttpGet]
        [Route("get-movies-by-genre")]
        public async Task<ActionResult<IEnumerable<MovieDTO>>> GetMoviesByGenre(int genreId)
        {
            var movies = await _movieService.GetMoviesByGenre(genreId);
            if (movies == null)
            {
                return NotFound();
            }
            return Ok(movies);
        }

        [HttpGet]
        [Route("get-movie-details")]
        public async Task<ActionResult<MovieDTO>> GetMovieDetails(int id)
        {
            var movie = await _movieService.GetMovieDetails(id);
            if (movie == null)
            {
                return NotFound();
            }
            return Ok(movie);
        }

        [HttpGet]
        [Route("get-movies-showing")]
        public async Task<ActionResult<IEnumerable<MovieDTO>>> GetMoviesShowing()
        {
            var movies = await _movieService.GetMoviesShowing();
            if (movies == null)
            {
                return NotFound();
            }
            return Ok(movies);
        }

        [HttpGet]
        [Route("get-top-movies")]
        public async Task<ActionResult<IEnumerable<MovieDTO>>> GetTopMovies()
        {
            var movies = await _movieService.GetTopMovies();
            if (movies == null)
            {
                return NotFound();
            }
            return Ok(movies);
        }

        [HttpGet]
        [Route("get-movies-end")]
        public async Task<ActionResult<IEnumerable<MovieDTO>>> GetMoviesEnd()
        {
            var movies = await _movieService.GetMoviesEnd();
            if (movies == null)
            {
                return NotFound();
            }
            return Ok(movies);
        }

        [HttpGet]
        [Route("get-movies-upcoming")]
        public async Task<ActionResult<IEnumerable<MovieDTO>>> GetMoviesUpcoming()
        {
            var movies = await _movieService.GetMoviesUpcoming();
            if (movies == null)
            {
                return NotFound();
            }
            return Ok(movies);
        }
    }

    /*    [Route("api/[controller]")]
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
                //var movieExists = _mapper.Map<Movie>(movie);
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

                return CreatedAtAction("GetMovie", new { id = newMovie.Id }, movie);
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

            [HttpGet]
            [Route("get-movies-by-genre")]
            public async Task<ActionResult<IEnumerable<MovieDTO>>> GetMoviesByGenre(int genreId)
            {
                var movies = await _unitOfWork.MovieRepository.GetMoviesByGenre(genreId);
                if (movies == null)
                {
                    return NotFound();
                }
                return _mapper.Map<List<MovieDTO>>(movies);
            }

            [HttpGet]
            [Route("get-movie-details")]
            public async Task<ActionResult<MovieDTO>> GetMovieDetails(int id)
            {
                var movie = await _unitOfWork.MovieRepository.GetMovieDetails(id);

                if (movie == null)
                {
                    return NotFound();
                }

                return _mapper.Map<MovieDTO>(movie);
            }

            [HttpGet]
            [Route("get-movies-showing")]
            public async Task<ActionResult<IEnumerable<MovieDTO>>> GetMoviesShowing()
            {
                var movies = await _unitOfWork.MovieRepository.GetMoviesShowing();
                if (movies == null)
                {
                    return NotFound();
                }
                return _mapper.Map<List<MovieDTO>>(movies);
            }

            [HttpGet]
            [Route("get-top-movies")]
            public async Task<ActionResult<IEnumerable<MovieDTO>>> GetTopMovies()
            {
                var movies = await _unitOfWork.MovieRepository.GetTopMovies();
                if (movies == null)
                {
                    return NotFound();
                }
                return _mapper.Map<List<MovieDTO>>(movies);
            }

            [HttpGet]
            [Route("get-movies-end")]
            public async Task<ActionResult<IEnumerable<MovieDTO>>> GetMoviesEnd()
            {
                var movies = await _unitOfWork.MovieRepository.GetMoviesEnd();
                if (movies == null)
                {
                    return NotFound();
                }
                return _mapper.Map<List<MovieDTO>>(movies);
            }

            [HttpGet]
            [Route("get-movies-upcoming")]
            public async Task<ActionResult<IEnumerable<MovieDTO>>> GetMoviesUpcoming()
            {
                var movies = await _unitOfWork.MovieRepository.GetMoviesUpcoming();
                if (movies == null)
                {
                    return NotFound();
                }
                return _mapper.Map<List<MovieDTO>>(movies);
            }


            private bool MovieExists(int id)
            {
                return _unitOfWork.MovieRepository.IsExists(id).Result;
            }
        }*/
}
