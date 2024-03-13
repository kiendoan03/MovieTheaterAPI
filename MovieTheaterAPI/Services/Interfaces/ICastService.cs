using Microsoft.AspNetCore.Mvc;
using MovieTheaterAPI.DTOs;
using MovieTheaterAPI.Entities;

namespace MovieTheaterAPI.Services.Interfaces
{
    public interface ICastService
    {
        Task<IEnumerable<CastDTO>> GetAll();
        Task<CastDTO> GetById(int id);
        Task<CastDTO> CreateCast(CastDTO castDTO, IFormFile file);
        Task Update(CastDTO castDTO, IFormFile file);
        Task Delete(int id);
        Task<bool> IsExists(int id);
        Task<IEnumerable<CastDTO>> GetMovieByCast(int castId);
    }

}
