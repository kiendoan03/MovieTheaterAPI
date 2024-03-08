using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Humanizer.Localisation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class CastsController : ControllerBase
    {
        private readonly ICastService _castService;

        public CastsController(ICastService castService)
        {
            _castService = castService;
        }

        [HttpGet]
        //[Authorize(Roles = "Staff, Manager")]
        public async Task<ActionResult<IEnumerable<CastDTO>>> GetCasts()
        {
            var casts = await _castService.GetAll();
            return Ok(casts);
        }

        [HttpGet("{id}")]
        //[Authorize(Roles = "Manager")]
        public async Task<ActionResult<CastDTO>> GetCast(int id)
        {
            var cast = await _castService.GetById(id);
            if (cast == null)
            {
                return NotFound();
            }
            return Ok(cast);
        }

        [HttpPost]
        //[Authorize(Roles = "Manager")]
        public async Task<ActionResult<CastDTO>> PostCast(CastDTO cast)
        {
            var newCast = await _castService.CreateCast(cast);
            return CreatedAtAction(nameof(GetCast), new { id = newCast.Id }, newCast);
        }

        [HttpPut("{id}")]
        //[Authorize(Roles = "Manager")]
        public async Task<IActionResult> PutCast(int id, CastDTO cast)
        {
            if (id != cast.Id)
            {
                return BadRequest();
            }

            await _castService.Update(cast);
            return NoContent();
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteCast(int id)
        {
            await _castService.Delete(id);
            return NoContent();
        }

        [HttpGet]
        [Route("get-movie-by-cast")]
        public async Task<ActionResult<IEnumerable<CastDTO>>> GetMovieByCast(int castId)
        {
            var movies = await _castService.GetMovieByCast(castId);
            if (movies == null)
            {
                return NotFound();
            }
            return Ok(movies);
        }
    }



    /*  [Route("api/[controller]")]
      [ApiController]
      public class CastsController : ControllerBase
      {
          private readonly IUnitOfWork _unitOfWork;
          private readonly IMapper _mapper;

          public CastsController(IMapper mapper, IUnitOfWork unitOfWork)
          {
              _mapper = mapper;
              _unitOfWork = unitOfWork;
          }

          // GET: api/Casts
          [HttpGet]
          //[Authorize]
          public async Task<ActionResult<IEnumerable<CastDTO>>> GetCasts()
          {
              var casts = await _unitOfWork.CastRepository.GetAll();
              return _mapper.Map<List<CastDTO>>(casts);
          }

          // GET: api/Casts/5
          [HttpGet("{id}")]
          public async Task<ActionResult<CastDTO>> GetCast(int id)
          {
              var cast = await _unitOfWork.CastRepository.GetById(id);

              if (cast == null)
              {
                  return NotFound();
              }

              return _mapper.Map<CastDTO>(cast);
          }

          [HttpGet]
          [Route("get-movie-by-cast")]
          public async Task<ActionResult<IEnumerable<CastDTO>>> GetMovieByCast(int castId)
          {
              var movies = await _unitOfWork.CastRepository.GetMovieByCast(castId);
              if (movies == null)
              {
                  return NotFound();
              }
              var movieDTOs = _mapper.Map<List<CastDTO>>(movies);

              return movieDTOs;
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

              await _unitOfWork.CastRepository.Update(updatedCast);

              try
              {
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

              await _unitOfWork.CastRepository.Add(newCast);
              await _unitOfWork.Save();

              return CreatedAtAction("GetCast", new { id = newCast.Id }, cast);
          }

          // DELETE: api/Casts/5
          [HttpDelete("{id}")]
          public async Task<IActionResult> DeleteCast(int id)
          {
              var cast = await _unitOfWork.CastRepository.GetById(id);
              if (cast == null)
              {
                  return NotFound();
              }

              await _unitOfWork.CastRepository.Delete(cast);
              await _unitOfWork.Save();

              return NoContent();
          }

          private bool CastExists(int id)
          {
              return _unitOfWork.CastRepository.IsExists(id).Result;
          }
      }*/
}
