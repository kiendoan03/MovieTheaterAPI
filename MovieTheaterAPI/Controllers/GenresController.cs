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
    public class GenresController : ControllerBase
    {
        private readonly IGenreService _genreService;

        public GenresController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        [HttpGet]
        //[Authorize (Roles = "Manager, Staff")]
        public async Task<ActionResult<IEnumerable<GenreDTO>>> GetGenres()
        {
            var genres = await _genreService.GetAllGenres();
            return Ok(genres);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<GenreDTO>> GetGenre(int id)
        {
            var genre = await _genreService.GetGenreById(id);
            if (genre == null)
            {
                return NotFound();
            }
            return Ok(genre);
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<GenreDTO>> PostGenre(GenreDTO genre)
        {
            var newGenre = await _genreService.CreateGenre(genre);
            return CreatedAtAction(nameof(GetGenre), new { id = newGenre.Id }, newGenre);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> PutGenre(int id, GenreDTO genre)
        {
            try
            {
                await _genreService.UpdateGenre(id, genre);
                return NoContent();
            }
            catch (ArgumentException)
            {
                return BadRequest("Id mismatch");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            await _genreService.DeleteGenre(id);
            return NoContent();
        }
    }



    /*    [Route("api/[controller]")]
        [ApiController]
        public class GenresController : ControllerBase
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;

            public GenresController(IMapper mapper, IUnitOfWork unitOfWork)
            {
                _mapper = mapper;
                _unitOfWork = unitOfWork;
            }

            // GET: api/Genres
            [HttpGet]
            public async Task<ActionResult<IEnumerable<GenreDTO>>> GetGenres()
            {
                var genres = await _unitOfWork.GenreRepository.GetAll();
                return _mapper.Map<List<GenreDTO>>(genres);
            }

            // GET: api/Genres/5
            [HttpGet("{id}")]
            public async Task<ActionResult<GenreDTO>> GetGenre(int id)
            {
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

                await _unitOfWork.GenreRepository.Update(update_genre);

                try
                {
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
                await _unitOfWork.GenreRepository.Add(new_genre);
                await _unitOfWork.Save();

                return CreatedAtAction("GetGenre", new { id = new_genre.Id }, genre);
            }

            // DELETE: api/Genres/5
            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteGenre(int id)
            {
                var genre = await _unitOfWork.GenreRepository.GetById(id);
                if (genre == null)
                {
                    return NotFound();
                }

                await _unitOfWork.GenreRepository.Delete(genre);
                await _unitOfWork.Save();

                return NoContent();
            }

            private bool GenreExists(int id)
            {
                return _unitOfWork.GenreRepository.IsExists(id).Result;
            }
        }*/
}
